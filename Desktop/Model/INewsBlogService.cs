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
        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}