using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;
using Xunit;

namespace Test
{
    public class ItemsControllserTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public ItemsControllserTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_Get_Post_Delete_Articles()
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Articles/");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Article>>(responseString);

            Assert.NotNull(responseObject);
            Assert.True(responseObject.Any());
            Assert.Equal(9, responseObject.Count());

            // Post
            Assert.Equal(24, _fixture.Context.Articles.Count());

            var test = new CreateDTO
            {
                Article = new ArticleDTO
                {
                    Id = 0,
                    Title = "testtitle",
                    Author = "",
                    UserId = "",
                    Date = DateTime.Now,
                    Summary = "sumsum",
                    Content = "concon",
                    Leading = false
                },
                Images = new List<PictureDTO>
                {

                }
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(test), Encoding.UTF8, "application/json");
            var response2 = await _fixture.Client.PostAsync("api/Articles/", content);

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Equal(25, _fixture.Context.Articles.Count());

            //Delete

            HttpResponseMessage response3 = await _fixture.Client.DeleteAsync("api/Articles/" + 1);

            response3.EnsureSuccessStatusCode();
            Assert.Equal(24, _fixture.Context.Articles.Count());

        }

        [Fact]
        public async void Test_PutArticle()
        {
            Assert.Equal(24, _fixture.Context.Articles.Count());

            var test = _fixture.Context.Articles.Find(2);

            var article = new ArticleDTO
            {
                Id = test.Id,
                Title = "testtitle",
                Author = test.Author,
                UserId = test.UserId,
                Date = DateTime.Now,
                Summary = "sumsum",
                Content = "concon",
                Leading = false
            };

            Assert.NotEqual(article.Title, test.Title);
            Assert.NotEqual(article.Summary, test.Summary);
            Assert.NotEqual(article.Content, test.Content);
            Assert.NotEqual(article.Leading, test.Leading);

            // Act

            var content1 = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PutAsync("api/Articles/", content1);

            // Assert
            response.EnsureSuccessStatusCode();

            var response2 = await _fixture.Client.GetAsync("api/Articles/" + 2);
            var responseString = await response2.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Article>(responseString);

            Assert.Equal(24, _fixture.Context.Articles.Count());
            Assert.Equal(article.Title, responseObject.Title);
            Assert.Equal(article.Summary, responseObject.Summary);
            Assert.Equal(article.Content, responseObject.Content);
            Assert.Equal(article.Leading, responseObject.Leading);
        }

        /*
        [Fact]
        public async void Test_PostItem_ShouldAddItem()
        {
            // Arrange
            Item item = new Item()
            {
                Name = "xy"
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync("api/Items", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(_fixture.Context.Items.FirstOrDefault(i => i.Name == "xy"));
        }*/
    }
}