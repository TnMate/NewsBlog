using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewsBlog.Persistence
{
    public class NewsBlogContext : IdentityDbContext<User>
    {
        public NewsBlogContext(DbContextOptions<NewsBlogContext> options)
            : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Article> Articles { get; set; }
    }
}
