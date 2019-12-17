using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace Desktop.Model
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

        public async Task<IEnumerable<ArticleDTO>> LoadArticlesAsync()
        {
            
            HttpResponseMessage response = await _client.GetAsync("api/Articles/");

            if (response.IsSuccessStatusCode)
            {
                var test = await response.Content.ReadAsStringAsync();
                var test2 = JsonConvert.DeserializeObject<dynamic>(test);

                var test3 = new List<ArticleDTO> { };
                foreach (var item in test2)
                {
                    var article = new ArticleDTO
                    {
                        Id = item.Value<int>("id"),
                        Title = item.Value<string>("title"),
                        Author = item.Value<string>("author"),
                        UserId = item.Value<string>("userid"),
                        Date = item.Value<DateTime>("date"),
                        Summary = item.Value<string>("summary"),
                        Content = item.Value<string>("content"),
                        Leading = item.Value<Boolean>("leading")
                    };
                    test3.Add(article);
                }
                return test3;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<Boolean> CreateArticle(ArticleDTO article)
        {
            
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Articles/", article);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<Boolean> UpdateArticle(ArticleDTO article)
        {

            HttpResponseMessage response = await _client.PutAsJsonAsync("api/Articles/", article);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<Boolean> DeleteArticle(int articleId)
        {
            HttpResponseMessage response = await _client.DeleteAsync("api/Articles/" + articleId);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

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