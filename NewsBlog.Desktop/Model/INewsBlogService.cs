using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace NewsBlog.Desktop.Model
{
    public interface INewsBlogService
    {
        bool IsUserLoggedIn { get; }
        Task<IEnumerable<Article>> LoadArticlesAsync();
        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}
