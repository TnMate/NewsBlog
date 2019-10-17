using System;
using System.Linq;
using System.Collections.Generic;

namespace NewsBlog.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(NewsBlogContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Articles.Any())
            {
                return;
            }

            // adatfeltöltés

            IList<Article> defaultArticless = new List<Article>
            {
                new Article
                {
                    Title = "Magyarok nyernek",
                    Date = DateTime.Now.AddDays(-1),
                    Summary = "Nyertek egy sporton belül",
                    Content = "A vívásban nyertek",
                    Leading = true
                },
                new Article
                {
                    Title = "Németek nyernek",
                    Date = DateTime.Now.AddDays(-2),
                    Summary = "A harmadik világtáncversenyen elsők lettek",
                    Content = "Megnyerték a versenyt, s bosszút álltak a második s az első világtáncversenyen elért csúfos vereségükért",
                    Leading = false
                },
                new Article
                {
                    Title = "Szerencse",
                    Date = DateTime.Now.AddDays(-3),
                    Summary = "Valaki visszaadta egy gyerek elveszett 50m forintját",
                    Content = "S az az ember kedvesen visszaadta a tulajdonosának, minden egyes fillérig, szerencsés fickó.",
                    Leading = false
                },
                new Article
                {
                    Title = "Balszerencse",
                    Date = DateTime.Now.AddDays(-4),
                    Summary = "Valaki megtalálta egy gyerek elveszett 50m forintját",
                    Content = "S az az ember kedvesen visszaadta a tulajdonosának, minden egyes fillérig, szegény felesége...",
                    Leading = false
                },
                new Article
                {
                    Title = "Title1",
                    Date = DateTime.Now.AddDays(-5),
                    Summary = "Summary1",
                    Content = "Content1",
                    Leading = true
                },
                new Article
                {
                    Title = "Title2",
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Summary2",
                    Content = "Content2",
                    Leading = false
                },
                new Article
                {
                    Title = "Title3",
                    Date = DateTime.Now.AddDays(-7),
                    Summary = "Summary3",
                    Content = "Content3",
                    Leading = false
                },
                new Article
                {
                    Title = "Title4",
                    Date = DateTime.Now.AddDays(-8),
                    Summary = "Summary4",
                    Content = "Content4",
                    Leading = false
                },
                new Article
                {
                    Title = "Title5",
                    Date = DateTime.Now.AddDays(-9),
                    Summary = "Summary5",
                    Content = "Content5",
                    Leading = false
                },
                new Article
                {
                    Title = "Title1.1",
                    Date = DateTime.Now.AddDays(-5),
                    Summary = "Summary1",
                    Content = "Content1",
                    Leading = true
                },
                new Article
                {
                    Title = "Title2.2",
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Summary2",
                    Content = "Content2",
                    Leading = false
                },
                new Article
                {
                    Title = "Title3.3",
                    Date = DateTime.Now.AddDays(-7),
                    Summary = "Summary3",
                    Content = "Content3",
                    Leading = false
                },
                new Article
                {
                    Title = "Title4.4",
                    Date = DateTime.Now.AddDays(-8),
                    Summary = "Summary4",
                    Content = "Content4",
                    Leading = false
                },
                new Article
                {
                    Title = "Title5.5",
                    Date = DateTime.Now.AddDays(-9),
                    Summary = "Summary5",
                    Content = "Content5",
                    Leading = false
                }
            };

            foreach (Article article in defaultArticless)
                context.Articles.Add(article);

            context.SaveChanges();
        }

    }
}
