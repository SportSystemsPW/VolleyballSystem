
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Discussion;
using Volleyball.DTO.Teams;
using Volleyball.DTO.Users;
using VolleyballDomain.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VolleyballBlazor.Infrastructure.Client.Services
{
    public interface IUserService
    {
        Task<ApiResponse> Register(RegisterDto registerDto);
        Task<ApiResponse<string>> Login(LoginDto loginDto);
        Task UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto);
        Task<ApiResponse<List<PositionDto>>> GetPositions();
        Task<ApiResponse<PlayerSummaryDto>> GetUserSummary();
        Task<ApiResponse<UserProfileDto>> GetUserProfile(int userId);
        Task<ApiResponse<bool>> IsTeamCaptain();
        Task<ApiResponse<UserProfileDto>> GetCurrentUserProfile();
        Task<ApiResponse> UpdateUser(UpdateUserDto userProfileDto);
    }

    public class UserService : IUserService
    {
        private HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }   

        public async Task<ApiResponse> Register(RegisterDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user/register", registerDto);
                return new ApiResponse(response);
            }
            catch (HttpRequestException)
            {
                return ApiResponse.NetworkErrorResponse;
            }
        }

        public async Task<ApiResponse<string>> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user/login", loginDto);
                return new ApiResponse<string>(response);
            }
            catch
            {
                return ApiResponse<string>.NetworkErrorResponse;
            }

        }

        public async Task<ApiResponse<List<PositionDto>>> GetPositions()
        {
            try
            {
                using (var response = await _httpClient.GetAsync("api/position"))
                {
                    return new ApiResponse<List<PositionDto>>(response);
                }
            }
            catch
            {
                return ApiResponse<List<PositionDto>>.NetworkErrorResponse;
            }
        }

        public async Task<ApiResponse<PlayerSummaryDto>> GetUserSummary()
        {
            try
            {
                using (var response = await _httpClient.GetAsync("api/user/usersummary"))
                {
                    return new ApiResponse<PlayerSummaryDto>(response);
                }
            }
            catch
            {
                return ApiResponse<PlayerSummaryDto>.NetworkErrorResponse;
            }
        }

        // get user profile
        public async Task<ApiResponse<UserProfileDto>> GetUserProfile(int userId)
        {
            try
            {
                using (var response = await _httpClient.GetAsync($"api/User/userprofile/{userId}"))
                {
                    return new ApiResponse<UserProfileDto>(response);
                }
            }
            catch
            {
                return ApiResponse<UserProfileDto>.NetworkErrorResponse;
            }
        }


        // check if user has a team
        public async Task<ApiResponse<bool>> IsTeamCaptain()
        {
            try
            {
                using (var response = await _httpClient.GetAsync($"api/User/isteamcaptain"))
                {
                    return new ApiResponse<bool>(response);
                }
            }
            catch
            {
                return ApiResponse<bool>.NetworkErrorResponse;
            }
        }

        // get current user profile
        public async Task<ApiResponse<UserProfileDto>> GetCurrentUserProfile()
        {
            try
            {
                using (var response = await _httpClient.GetAsync($"api/User/myprofile"))
                {
                    return new ApiResponse<UserProfileDto>(response);
                }
            }
            catch
            {
                return ApiResponse<UserProfileDto>.NetworkErrorResponse;
            }
        }

        // update user profile
        public async Task<ApiResponse> UpdateUser(UpdateUserDto userProfileDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/user/updateuserdata", userProfileDto);
                return new ApiResponse(response);
            }
            catch (HttpRequestException)
            {
                return ApiResponse.NetworkErrorResponse;
            }
        }

        // TODO
        public async Task UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto)
        {
            await _httpClient.PostAsJsonAsync($"api/user/{userId}/updatePassword", updatePasswordDto);
        }

    }
}
