using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace NewsBlog.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(NewsBlogContext context, string imageDirectory = null)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Articles.Any())
            {
                return;
            }

            // adatfeltöltés

            #region Articles

            IList<Article> defaultArticles = new List<Article>
            {
                new Article
                {
                    Title = "Magyarok nyernek",
                    Author = "Someone1",
                    Date = DateTime.Now.AddDays(-1),
                    Summary = "Nyertek egy sporton belül",
                    Content = "A vívásban nyertek",
                    Leading = false
                },
                new Article
                {
                    Title = "Németek nyernek",
                    Author = "Someone2",
                    Date = DateTime.Now.AddDays(-2),
                    Summary = "A harmadik világtáncversenyen elsők lettek",
                    Content = "Megnyerték a versenyt, s bosszút álltak a második s az első világtáncversenyen elért csúfos vereségükért",
                    Leading = true
                },
                new Article
                {
                    Title = "Szerencse",
                    Author = "Someone1",
                    Date = DateTime.Now.AddDays(-3),
                    Summary = "Valaki visszaadta egy gyerek elveszett 50m forintját",
                    Content = "S az az ember kedvesen visszaadta a tulajdonosának, minden egyes fillérig, szerencsés fickó.",
                    Leading = false
                },
                new Article
                {
                    Title = "Balszerencse",
                    Author = "Someone1",
                    Date = DateTime.Now.AddDays(-4),
                    Summary = "Valaki megtalálta egy gyerek elveszett 50m forintját",
                    Content = "S az az ember kedvesen visszaadta a tulajdonosának, minden egyes fillérig, szegény felesége...",
                    Leading = false
                },
                new Article
                {
                    Title = "Title1",
                    Author = "Someone2",
                    Date = DateTime.Now.AddDays(-5),
                    Summary = "Summary1",
                    Content = "Content1",
                    Leading = true
                },
                new Article
                {
                    Title = "Title2",
                    Author = "Someone3",
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Summary2",
                    Content = "Content2",
                    Leading = false
                },
                new Article
                {
                    Title = "Title3",
                    Author = "Someone1",
                    Date = DateTime.Now.AddDays(-7),
                    Summary = "Summary3",
                    Content = "Content3",
                    Leading = false
                },
                new Article
                {
                    Title = "Title4",
                    Author = "Someone3",
                    Date = DateTime.Now.AddDays(-8),
                    Summary = "Summary4",
                    Content = "Content4",
                    Leading = false
                },
                new Article
                {
                    Title = "Title5",
                    Author = "Someone1",
                    Date = DateTime.Now.AddDays(-9),
                    Summary = "Summary5",
                    Content = "Content5",
                    Leading = false
                },
                new Article
                {
                    Title = "Title1.1",
                    Author = "Someone2",
                    Date = DateTime.Now.AddDays(-5),
                    Summary = "Summary1",
                    Content = "Content1",
                    Leading = true
                },
                new Article
                {
                    Title = "Title2.2",
                    Author = "Someone2",
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Summary2",
                    Content = "Content2",
                    Leading = false
                },
                new Article
                {
                    Title = "Title3.3",
                    Author = "Someone2",
                    Date = DateTime.Now.AddDays(-7),
                    Summary = "Summary3",
                    Content = "Content3",
                    Leading = false
                },
                new Article
                {
                    Title = "Title4.4",
                    Author = "Someone4",
                    Date = DateTime.Now.AddDays(-8),
                    Summary = "Summary4",
                    Content = "Content4",
                    Leading = false
                },
                new Article
                {
                    Title = "Title5.5",
                    Author = "Someone2",
                    Date = DateTime.Now.AddDays(-9),
                    Summary = "Summary5",
                    Content = "Content5",
                    Leading = false
                }
            };

            foreach (Article article in defaultArticles)
                context.Articles.Add(article);

            #endregion

            #region Pictures

            if (imageDirectory != null && Directory.Exists(imageDirectory))
            {
                IList<Picture> defaultPictures = new List<Picture>();

                var path = Path.Combine(imageDirectory, "one.png");
                //var largePath = Path.Combine(imageDirectory, "one_big.png");
                if (File.Exists(path) /*&& File.Exists(largePath)*/)
                {
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = 1,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllBytes(largePath)
                    });
                }
                path = Path.Combine(imageDirectory, "two.png");
                //var largePath = Path.Combine(imageDirectory, "two_big.png");
                if (File.Exists(path) /*&& File.Exists(largePath)*/)
                {
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = 2,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllBytes(largePath)
                    });
                }

                path = Path.Combine(imageDirectory, "third.png");
                //var largePath = Path.Combine(imageDirectory, "third_big.png");
                if (File.Exists(path) /*&& File.Exists(largePath)*/)
                {
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = 3,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllBytes(largePath)
                    });
                }

                foreach (Picture picture in defaultPictures)
                    context.Pictures.Add(picture);
            }

            #endregion

            context.SaveChanges();
        }

    }
}
