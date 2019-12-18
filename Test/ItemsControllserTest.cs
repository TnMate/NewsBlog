using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NewsBlog.Persistence;
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
        public async void Test_GetListItems_WithInvalidId_ReturnsEmptyList()
        {
            // Arrange
            int userID = 66666;

            // Act
            var response = await _fixture.Client.GetAsync("api/Articles/");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Article>>(responseString);

            Assert.NotNull(responseObject);
            Assert.False(responseObject.Any());
        }
        /*
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Test_GetListItems_ReturnsAllRelevantItems(int listId)
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Items/?listId=" + listId);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Item>>(responseString);

            Assert.NotNull(responseObject);
            Assert.All(responseObject, item => Assert.Equal(listId, item.ListId));
            Assert.Equal(_fixture.Context.Items.Count(i => i.ListId == listId), responseObject.Count());
        }


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