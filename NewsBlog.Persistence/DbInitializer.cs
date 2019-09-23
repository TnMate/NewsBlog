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

        private static void SeedArticles()
        {

            var articles = new Article[]
            {
                new Article {Title = "Cavallino"},
                new Article {Title = "Lido di Jesolo"},
            };
            foreach (Article c in articles)
            {
                _context.Cities.Add(c);
            }

            _context.SaveChanges();
        }

    }
}
