using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Discussion;

namespace VolleyballBlazor.Infrastructure.Client.Services
{
    public interface IDiscussionService
    {
        Task<ApiResponse<List<ArticleDto>>> GetAllArticles();
        Task<ApiResponse> AddArticleComment(NewCommentDto comment);
        Task<ApiResponse> AddProfileComment(NewCommentDto comment);
        Task<ApiResponse> AddTeamComment(NewCommentDto comment);
        Task<ApiResponse> AddMatchComment(NewCommentDto comment);
        Task<ApiResponse<List<CommentDto>>> GetProfileComments(int id);
        Task<ApiResponse<List<CommentDto>>> GetTeamComments(int id);
        Task<ApiResponse<List<CommentDto>>> GetArticleComments(int id);
        Task<ApiResponse<List<CommentDto>>> GetMatchComments(int id);
        Task<ApiResponse<ArticleDto>> GetArticle(int id);
        Task<ApiResponse<List<LogDto>>> GetRecentLogs();
    }

    public class DiscussionService : IDiscussionService
    {
        private HttpClient _httpClient;

        public DiscussionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<ArticleDto>>> GetAllArticles()
        {
            var response = await _httpClient.GetAsync($"api/article");
            return new ApiResponse<List<ArticleDto>>(response);
        }

        public async Task<ApiResponse<ArticleDto>> GetArticle(int id)
        {
            var response = await _httpClient.GetAsync($"api/article/{id}");
            return new ApiResponse<ArticleDto>(response);
        }

        public async Task<ApiResponse> AddArticleComment(NewCommentDto comment)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/discussion/articlecomment", comment);
            return new ApiResponse(response);
        }

        public async Task<ApiResponse> AddProfileComment(NewCommentDto comment)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/discussion/profilecomment", comment);
            return new ApiResponse(response);
        }

        public async Task<ApiResponse> AddTeamComment(NewCommentDto comment)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/discussion/teamcomment", comment);
            return new ApiResponse(response);
        }

        public async Task<ApiResponse> AddMatchComment(NewCommentDto comment)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/discussion/matchcomment", comment);
            return new ApiResponse(response);
        }

        public async Task<ApiResponse<List<CommentDto>>> GetProfileComments(int id)
        {
            var response = await _httpClient.GetAsync($"api/discussion/profilecomments/{id}");
            return new ApiResponse<List<CommentDto>>(response);
        }

        public async Task<ApiResponse<List<CommentDto>>> GetTeamComments(int id)
        {
            var response = await _httpClient.GetAsync($"api/discussion/teamcomments/{id}");
            return new ApiResponse<List<CommentDto>>(response);
        }

        public async Task<ApiResponse<List<CommentDto>>> GetArticleComments(int id)
        {
            var response = await _httpClient.GetAsync($"api/discussion/articlecomments/{id}");
            return new ApiResponse<List<CommentDto>>(response);
        }

        public async Task<ApiResponse<List<CommentDto>>> GetMatchComments(int id)
        {
            var response = await _httpClient.GetAsync($"api/discussion/matchcomments/{id}");
            return new ApiResponse<List<CommentDto>>(response);
        }


        // get logs
        public async Task<ApiResponse<List<LogDto>>> GetRecentLogs()
        {
            try
            {
                using (var response = await _httpClient.GetAsync($"api/discussion/logs"))
                {
                    return new ApiResponse<List<LogDto>>(response);
                }
            }
            catch
            {
                return ApiResponse<List<LogDto>>.NetworkErrorResponse;
            }
        }
    }
}
