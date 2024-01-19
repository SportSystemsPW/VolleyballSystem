using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Volleyball.DTO.Teams;
using Volleyball.Infrastructure.Database.Models;
using VolleyballDomain.Shared;

namespace Volleyball.DbServices.Services
{
    public class TeamDbService
    {
        private readonly VolleyballContext context;

        public TeamDbService(VolleyballContext context)
        {
            this.context = context;
        }

        public TeamDbService()
        {
            context = new VolleyballContext();
        }

        public async Task<ServiceResponse<List<TeamSummaryDto>>> GetAllTeamsAsync()
        {
            var response = new ServiceResponse<List<TeamSummaryDto>>();
            response.Data = (await context.Teams.ToListAsync()).Select(t => (TeamSummaryDto)t).ToList();
            return response;
        }

        public async Task<ServiceResponse<TeamDto>> GetTeamByIdAsync(int id)
        {
            var response = new ServiceResponse<TeamDto>();
            var team = await context.Teams
                .Include(t => t.TeamPlayers).ThenInclude(tp => tp.Player).ThenInclude(p => p.Position)
                .Include(t => t.Captain).ThenInclude(c => c.Position)
                .Include(t => t.League)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                response.Success = false;
                response.Message = "Team not found";
                return response;
            }

            response.Data = TeamDto.GenerateTeamDto(team);

            return response;
        }

        public async Task<ServiceResponse<TeamDto>> AddTeam(NewTeamDto team, string userId)
        {
            var response = new ServiceResponse<TeamDto>();

            var teamPlayers = new List<TeamPlayer>();
            var newUsersToSendInvitation = new List<TeamPlayerDto>();

            foreach (var player in team.Players)
            {
                // if players email is not null, try to match them with existing account
                if (player.Email != null)
                {
                    var user = await context.Users.Include(u => u.Credentials).FirstOrDefaultAsync(u => u.Credentials.Email == player.Email);
                    if (user != null)
                    {
                        teamPlayers.Add(new TeamPlayer
                        {
                            Player = user,
                            JoinDate = DateTime.Now
                        });
                        continue;
                    }
                }

                // if cant match, add them to Users
                newUsersToSendInvitation.Add(player);
                teamPlayers.Add(new TeamPlayer
                {
                    Player = new User
                    {
                        FirstName = player.FirstName,
                        LastName = player.LastName,
                        Height = (byte?)player.Height,
                        JerseyNumber = (byte?)player.JerseyNumber,
                        PositionId = player.PositionId,
                        Credentials = null
                    },
                    JoinDate = DateTime.Now
                });
            }

            var captain = (await context.Credentials.Include(c => c.User).FirstOrDefaultAsync(u => u.Email == userId))?.User;

            if (captain == null)
            {
                response.Success = false;
                response.Message = "Captain not found";
                return response;
            }

            var newTeam = new Team
            {
                Name = team.Name,
                CreationDate = DateTime.Now,
                Image = team.Image,
                LeagueId = 1,
                CaptainId = captain.Id,
                TeamPlayers = teamPlayers,
                Email = team.Email,
                Logo = team.Logo,
                Phone = team.Phone,
                TeamDescription = team.TeamDescription,
                Website = team.Website,
                IsReportedToPlay = false
            };

            try
            {

                await context.Teams.AddAsync(newTeam);
                await context.SaveChangesAsync();

                foreach (var player in teamPlayers)
                {
                    player.Team = newTeam;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return response;
            }

            foreach (var newUser in newUsersToSendInvitation)
            {
                SendEmailAddedToTeam(newUser);
            }

            return response;
        }

        public async Task<ServiceResponse<ManageTeamDto>> UpdateTeam(ManageTeamDto team, string email)
        {
            var response = new ServiceResponse<ManageTeamDto>();

            var teamToUpdate = await context.Teams
                .Include(u => u.Captain)
                .Include(t => t.TeamPlayers).ThenInclude(t => t.Player).ThenInclude(p => p.Credentials)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            if (teamToUpdate == null)
            {
                response.Success = false;
                response.Message = "Team not found";
                return response;
            }

            teamToUpdate.Image = team.Photo;
            teamToUpdate.Email = team.Email;
            teamToUpdate.Logo = team.Logo;
            teamToUpdate.Phone = team.Phone;
            teamToUpdate.TeamDescription = team.TeamDescription;
            teamToUpdate.Website = team.Website;

            teamToUpdate.Captain.JerseyNumber = (byte?)team.Captain.JerseyNumber;
            teamToUpdate.Captain.Height = (byte?)team.Captain.Height;
            teamToUpdate.Captain.PositionId = team.Captain.PositionId;
            teamToUpdate.Captain.Gender = team.Captain.Gender;

            foreach (var player in team.Players)
            {
                var playerToUpdate = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Player.Id == player.Id);
                if (playerToUpdate != null)
                {
                    playerToUpdate.Player.JerseyNumber = (byte?)player.JerseyNumber;
                    playerToUpdate.Player.Height = (byte?)player.Height;
                    playerToUpdate.Player.PositionId = player.PositionId;
                    playerToUpdate.Player.Gender = player.Gender;
                    if (playerToUpdate.Player.Credentials == null)
                    {
                        playerToUpdate.Player.AdditionalEmail = player.Email;
                    }
                }
            }

            teamToUpdate.TeamPlayers.Concat(team.NewPlayers.Select(p => new TeamPlayer
            {
                Player = new User
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Height = (byte?)p.Height,
                    JerseyNumber = (byte?)p.JerseyNumber,
                    PositionId = p.PositionId,
                    Credentials = null
                },
                JoinDate = DateTime.Now
            }).ToList());

            foreach (var player in team.NewPlayers)
            {
                var user = await context.Credentials.Include(c => c.User).FirstOrDefaultAsync(c => c.Email == player.Email);
                if (user != null)
                {
                    teamToUpdate.TeamPlayers.Add(new TeamPlayer
                    {
                        Player = user.User,
                        JoinDate = DateTime.Now
                    });
                }
                else
                {
                    teamToUpdate.TeamPlayers.Add(new TeamPlayer
                    {
                        Player = new User
                        {
                            FirstName = player.FirstName,
                            LastName = player.LastName,
                            Height = (byte?)player.Height,
                            JerseyNumber = (byte?)player.JerseyNumber,
                            PositionId = player.PositionId,
                            Credentials = null,
                            AdditionalEmail = player.Email
                        },
                        JoinDate = DateTime.Now
                    });

                    if (player.Email != null)
                    {
                        SendEmailAddedToTeam(player);
                    }
                }
            }

            foreach (var player in team.RemovedPlayers)
            {
                var playerToRemove = teamToUpdate.TeamPlayers.FirstOrDefault(p => p.Player.Id == player.Id);
                if (playerToRemove != null)
                {
                    teamToUpdate.TeamPlayers.Remove(playerToRemove);
                }
            }

            try
            {
                context.Update(teamToUpdate);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse> UpdateTeamPlayer(PlayerSummaryDto userSummary)
        {
            var response = new ServiceResponse();

            var player = context.Users.FirstOrDefault(u => u.Id == userSummary.Id);

            if (player == null)
            {
                response.Success = false;
                response.Message = "Player not found";
                return response;
            }

            player.JerseyNumber = (byte?)userSummary.JerseyNumber;
            player.Height = (byte?)userSummary.Height;
            player.Position = await context.Positions.FirstOrDefaultAsync(p => p.Name == userSummary.PositionName);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<ManagedTeamDataDto>> GetTeamByCaptain(string email)
        {
            var response = new ServiceResponse<ManagedTeamDataDto>();

            var team = await context.Teams
                .Include(t => t.League)
                .Include(u => u.Captain).ThenInclude(c => c.Credentials)
                .Include(t => t.TeamPlayers).ThenInclude(t => t.Player).ThenInclude(p => p.Credentials)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            if (team == null)
            {
                response.Success = false;
                response.Message = "Team not found";
                return response;
            }

            response.Data = (ManagedTeamDataDto)team;
            return response;
        }

        // get team by league id
        public async Task<ServiceResponse<List<TeamDto>>> GetTeamsByLeagueId(int leagueId)
        {
            var response = new ServiceResponse<List<TeamDto>>();
            var teams = await context.Teams
                .Include(t => t.League)
                .Include(t => t.TeamPlayers).ThenInclude(p => p.Player).ThenInclude(p => p.Position)
                .Include(t => t.Captain)
                .Where(t => t.League != null && t.League.Id == leagueId).ToListAsync();
            if (teams == null)
            {
                response.Success = false;
                response.Message = "Teams not found";
                return response;
            }
            response.Data = teams.Select(t => (TeamDto)t).ToList();
            return response;
        }

        //   update captain
        public async Task<ServiceResponse> UpdateCaptain(int newCaptainId, string email)
        {
            var response = new ServiceResponse();
            var team = await context.Teams
                .Include(t => t.League)
                .Include(u => u.Captain)
                .Include(t => t.TeamPlayers)
                .FirstOrDefaultAsync(t => t.Captain.Credentials!.Email == email);

            if (team == null)
            {
                response.Success = false;
                response.Message = "Team not found";
                return response;
            }

            var newCaptain = await context.Users
                .FirstOrDefaultAsync(u => u.Id == newCaptainId);

            var oldCaptainId = team.Captain.Id;

            if (newCaptain == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";
                return response;
            }
            team.Captain = newCaptain;
            team.TeamPlayers.Remove(team.TeamPlayers.FirstOrDefault(p => p.PlayerId == newCaptainId)!);


            team.TeamPlayers.Add(new TeamPlayer
            {
                PlayerId = oldCaptainId,
                JoinDate = DateTime.Now
            });

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        // get leagues
        public async Task<ServiceResponse<List<LeagueDto>>> GetLeagues()
        {
            var response = new ServiceResponse<List<LeagueDto>>();
            var leagues = await context.Leagues.ToListAsync();
            if (leagues == null)
            {
                response.Success = false;
                response.Message = "Leagues not found";
                return response;
            }
            response.Data = leagues.Select(l => (LeagueDto)l).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<SeasonDto>>> GetSeasons()
        {
            var response = new ServiceResponse<List<SeasonDto>>();
            var seasons = await context.Seasons.ToListAsync();
            if (seasons == null)
            {
                response.Success = false;
                response.Message = "Seasons not found";
                return response;
            }

            response.Data = seasons.OrderByDescending(s => s.Id).Select(s => (SeasonDto)s).ToList();

            return response;
        }


        private void SendEmailAddedToTeam(TeamPlayerDto player)
        {
            // TODO: Send email to player
        }
    }
}
