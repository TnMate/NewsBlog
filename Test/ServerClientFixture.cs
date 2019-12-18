using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;
using WebApi;

namespace Test
{
    public class ServerClientFixture : IDisposable
    {
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public NewsBlogContext Context { get; private set; }

        public ServerClientFixture()
        {
            // Arrange
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());

            Context = Server.Host.Services.GetRequiredService<NewsBlogContext>();
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}