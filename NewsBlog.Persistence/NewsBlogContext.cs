using System;
using Microsoft.EntityFrameworkCore;

namespace NewsBlog.Persistence
{
    public class NewsBlogContext : DbContext
    {
        public NewsBlogContext(DbContextOptions<NewsBlogContext> options)
            : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
