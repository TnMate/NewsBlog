using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsBlog.Persistence;

namespace Test
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NewsBlogContext>(options =>
                options.UseInMemoryDatabase("TestingDB"));


            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<NewsBlogContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NewsBlogContext context)
        {
            app.UseAuthentication();
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>(); // automatikus "bejelentkezés"

            app.UseMvc();
            TestDbInitializer.Initialize(context);
        }
    }
}