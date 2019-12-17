using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace Desktop.Model
{
    public interface INewsBlogService
    {
        bool IsUserLoggedIn { get; }
        Task<IEnumerable<ArticleDTO>> LoadArticlesAsync();
        Task<IEnumerable<PictureDTO>> LoadPicturesAsync(int articleId);
        Task<Boolean> UpdateArticle(ArticleDTO article);
        Task<Boolean> CreateArticle(ArticleDTO article);
        Task<Boolean> DeleteArticle(int articleId);
        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}