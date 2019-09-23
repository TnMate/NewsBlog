using System;
using System.Linq;

namespace NewsBlog.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(NewsBlogContext context)
        {
            context.Database.EnsureCreated();

            if (context.Articles.Any())
            {
                return;
            }

            // adatfeltöltés

            context.SaveChanges();
        }

    }
}
