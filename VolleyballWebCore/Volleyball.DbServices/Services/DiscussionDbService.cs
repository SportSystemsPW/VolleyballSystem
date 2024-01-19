using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Discussion;
using Volleyball.Infrastructure.Database.Models;
using VolleyballDomain.Shared;

namespace Volleyball.DbServices.Services
{
    public class DiscussionDbService
    {
        private readonly VolleyballContext context;

        public DiscussionDbService()
        {
            context = new VolleyballContext();
        }

        public DiscussionDbService(VolleyballContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<List<CommentDto>>> GetProfileComments(int profileId)
        {
            return await GetComments(profileId, CommentLocations.Player);
        }

        public async Task<ServiceResponse<List<CommentDto>>> GetTeamComments(int teamId)
        {
            return await GetComments(teamId, CommentLocations.Team);
        }

        public async Task<ServiceResponse<List<CommentDto>>> GetArticleComments(int articleId)
        {
            return await GetComments(articleId, CommentLocations.Article);
        }

        public async Task<ServiceResponse<List<CommentDto>>> GetMatchComments(int matchId)
        {
            return await GetComments(matchId, CommentLocations.Match);
        }

        public async Task<ServiceResponse> AddArticleComment(NewCommentDto comment, string authorEmail)
        {
            return await AddComment(comment, authorEmail, CommentLocations.Article);
        }

        public async Task<ServiceResponse> AddProfileComment(NewCommentDto comment, string authorEmail)
        {
            return await AddComment(comment, authorEmail, CommentLocations.Player);
        }

        public async Task<ServiceResponse> AddTeamComment(NewCommentDto comment, string authorEmail)
        {
            return await AddComment(comment, authorEmail, CommentLocations.Team);
        }

        public async Task<ServiceResponse> AddMatchComment(NewCommentDto comment, string authorEmail)
        {
            return await AddComment(comment, authorEmail, CommentLocations.Match);
        }

        public async Task<ServiceResponse> SendMessage(NewCommentDto newCommentDto, string authorEmail)
        {
            return await AddComment(newCommentDto, authorEmail, CommentLocations.PrivateMessage);
        }


        public async Task<ServiceResponse<List<LogDto>>> GetRecentLogs(int amount)
        {
            var response = new ServiceResponse<List<LogDto>>();
            var logs = await context.Logs.Include(l => l.PersonalLogs).ThenInclude(l => l.User).OrderByDescending(l => l.Date).Take(amount).ToListAsync();
            response.Data = logs.Select(l => (LogDto)l).ToList();
            return response;
        }


        private async Task<ServiceResponse<List<CommentDto>>> GetComments(int locationId, CommentLocations location)
        {
            var response = new ServiceResponse<List<CommentDto>>();

            var comments = await context.Comments.Include(c => c.Author).Where(x => x.ContentId == locationId && x.CommentLocation == GetLocation(location)).OrderByDescending(c => c.CreationDate).ToListAsync();

            response.Data = comments.Select(x => (CommentDto)x).ToList();

            return response;
        }
        private async Task<ServiceResponse> AddComment(NewCommentDto comment, string authorEmail, CommentLocations location)
        {
            var response = new ServiceResponse();
            try
            {
                var author = await context.Users.Include(u => u.Credentials)
                                    .FirstOrDefaultAsync(u => u.Credentials != null && u.Credentials.Email == authorEmail);
                if (author == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Nie znaleziono użytkownika"
                    };
                }
                var commentLocation = GetLocation(location);
                if (commentLocation == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Nie znaleziono miejsca komentarza"
                    };
                }

                var newComment = new Comment
                {
                    Author = author,
                    Content = comment.Content,
                    ContentId = comment.ContentLocationId,
                    CommentLocation = commentLocation,
                    CreationDate = DateTime.Now,
                    Active = true
                };
                await context.Comments.AddAsync(newComment);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        private CommentLocation? GetLocation(CommentLocations location)
        {
            return context.CommentLocations.FirstOrDefault(x => x.Name == location.ToString());
        }
    }
}
