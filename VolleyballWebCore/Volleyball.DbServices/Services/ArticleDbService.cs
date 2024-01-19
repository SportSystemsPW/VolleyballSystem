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
    public class ArticleDbService
    {
        private readonly VolleyballContext context;

        public ArticleDbService()
        {
            context = new VolleyballContext();
        }

        public ArticleDbService(VolleyballContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<List<ArticleDto>>> GetAllArticles()
        {
            var response = new ServiceResponse<List<ArticleDto>>();

            try
            {
                response.Data = (await context.Articles.Include(a => a.Author).OrderByDescending(a => a.CreationDate).ToListAsync()).Select(a => (ArticleDto)a).ToList();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<ArticleDto>> GetArticle(int id)
        {
            var response = new ServiceResponse<ArticleDto>();
            try
            {
                var article = await context.Articles.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
                if (article == null)
                {
                    return new ServiceResponse<ArticleDto>
                    {
                        Success = false,
                        Message = "Nie znaleziono artykułu"
                    };
                }
                response.Data = (ArticleDto)article;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse> AddArticle(NewArticleDto article, string authorEmail)
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

                var newArticle = new Article
                {
                    Content = article.Content,
                    Title = article.Title,
                    Image = article.Image,
                    Author = author,
                    CreationDate = DateTime.Now,
                    IsActive = true
                };

                await context.Articles.AddAsync(newArticle);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse> UpdateArticle(UpdateArticleDto article)
        {
            var response = new ServiceResponse();

            try
            {
                var articleToUpdate = await context.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);
                if (articleToUpdate == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Nie znaleziono artykułu"
                    };
                }

                articleToUpdate.Content = article.Content;
                articleToUpdate.Title = article.Title;
                articleToUpdate.Image = article.Image;

                context.Update(articleToUpdate);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }
    }
}
