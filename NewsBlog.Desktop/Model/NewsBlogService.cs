using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;
using Newtonsoft.Json;

namespace NewsBlog.Desktop.Model
{
    public class NewsBlogService : INewsBlogService
    {
        private readonly HttpClient _client;

        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn => _isUserLoggedIn;

        public NewsBlogService(string baseAddress)
        {
            _isUserLoggedIn = false;
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<Article>> LoadArticlesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Articles/");

            if (response.IsSuccessStatusCode)
            {
                var test = await response.Content.ReadAsStringAsync();
                var test2 = JsonConvert.DeserializeObject<dynamic>(test);

                var test3 = new List<Article>{ };
                var article = new Article { };
                foreach (var item in test2)
                {
                    var test4 = item.ToList();

                    article = new Article
                    {
                        Id = item.Value<int>("Id"),
                        Title = item.Value<string>("Title"),
                        Author = item.Value<string>("Author"),
                        UserId = item.Value<string>("Userid"),
                        Date = item.Value<DateTime>("Date"),
                        Summary = item.Value<string>("Summary"),
                        Content = item.Value<string>("Content"),
                        Leading = item.Value<Boolean>("Leading")
                    };
                    article.Id = item.Value<int>("id");
                    test3.Add(article);
                }
                return test3;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

/*        public async Task<IEnumerable<Article>> LoadArticleAsync(int articleId)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Articles/?articleId={articleId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Article>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }*/

        public async Task<bool> LoginAsync(string name, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = name,
                Password = password
            };

            //HttpResponseMessage response = await _client.PostAsync("api/Account/Login",
            //    new StringContent(JsonConvert.SerializeObject(user),
            //        Encoding.UTF8,
            //        "application/json"));
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Login", user);

            if (response.IsSuccessStatusCode)
            {
                _isUserLoggedIn = true;
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<bool> LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Signout", "");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
    }
}
