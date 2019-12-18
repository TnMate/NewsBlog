using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NewsBlog.Persistence
{
    public static class DbInitializer
    {

        public static void Initialize(NewsBlogContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string imageDirectory = null)
        {

            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Articles.Any())
            {
                return;
            }

            // adatfeltöltés

            #region Users

            var user1 = new User()
            {
                UserName = "asd1",
                Name = "Ember1"
            };
            var user2 = new User()
            {
                UserName = "asd2",
                Name = "Ember2"
            };
            var user3 = new User()
            {
                UserName = "asd3",
                Name = "Ember3"
            };
            var user4 = new User()
            {
                UserName = "asd4",
                Name = "Ember4"
            };
            Task.Run(async () =>
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));

                await userManager.CreateAsync(user1, "123");
                await userManager.CreateAsync(user2, "123");
                await userManager.CreateAsync(user3, "123");
                await userManager.CreateAsync(user4, "123");

                await userManager.AddToRoleAsync(user1, "admin");
                await userManager.AddToRoleAsync(user2, "admin");
                await userManager.AddToRoleAsync(user3, "admin");
                await userManager.AddToRoleAsync(user4, "admin");
            }).GetAwaiter().GetResult();

            #endregion

            #region Articles

            IList<Article> defaultArticles = new List<Article>
            {
                new Article
                {
                    Title = "Title1.1",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-5),
                    Summary = "Summary1",
                    Content = "Content1",
                    Leading = true
                },
                new Article
                {
                    Title = "Title1",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-5),
                    Summary = "Summary1",
                    Content = "Content1",
                    Leading = true
                },
                new Article
                {
                    Title = "Németek nyernek",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-2),
                    Summary = "A harmadik világtáncversenyen elsők lettek",
                    Content = "Megnyerték a versenyt, s bosszút álltak a második s az első világtáncversenyen elért csúfos vereségükért",
                    Leading = true
                },
                new Article
                {
                    Title = "Csontok",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Hol ásták el a csontokat",
                    Content = "Azt hallottam hogy a korábbi kisállatok valamiféle csontokat ástak el valahova a kertben. Meg kell találnom őket... hol lehetnek... hmm fel kéne ásnom az egész kertet hogy megleljem őket.",
                    Leading = true
                },
                new Article
                {
                    Title = "Magyarok nyernek",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-1),
                    Summary = "Nyertek egy sporton belül",
                    Content = "A vívásban nyertek",
                    Leading = false
                },
                new Article
                {
                    Title = "Szerencse",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-3),
                    Summary = "Valaki visszaadta egy gyerek elveszett 50m forintját",
                    Content = "S az az ember kedvesen visszaadta a tulajdonosának, minden egyes fillérig, szerencsés fickó.",
                    Leading = false
                },
                new Article
                {
                    Title = "Balszerencse",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-4),
                    Summary = "Valaki megtalálta egy gyerek elveszett 50m forintját",
                    Content = "S az az ember kedvesen visszaadta a tulajdonosának, minden egyes fillérig, szegény felesége...",
                    Leading = false
                },
                new Article
                {
                    Title = "Title2",
                    Author = user3.Name,
                    UserId = user3.Id,
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Summary2",
                    Content = "Content2",
                    Leading = false
                },
                new Article
                {
                    Title = "Title3",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-7),
                    Summary = "Summary3",
                    Content = "Content3",
                    Leading = false
                },
                new Article
                {
                    Title = "Title4",
                    Author = user3.Name,
                    UserId = user3.Id,
                    Date = DateTime.Now.AddDays(-8),
                    Summary = "Summary4",
                    Content = "Content4",
                    Leading = false
                },
                new Article
                {
                    Title = "Title5",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-9),
                    Summary = "Summary5",
                    Content = "Content5",
                    Leading = false
                },
                new Article
                {
                    Title = "Title2.2",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Summary2",
                    Content = "Content2",
                    Leading = false
                },
                new Article
                {
                    Title = "Title3.3",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-7),
                    Summary = "Summary3",
                    Content = "Content3",
                    Leading = false
                },
                new Article
                {
                    Title = "Title4.4",
                    Author = user4.Name,
                    UserId = user4.Id,
                    Date = DateTime.Now.AddDays(-8),
                    Summary = "Summary4",
                    Content = "Content4",
                    Leading = false
                },
                new Article
                {
                    Title = "Valami amerika",
                    Author = user4.Name,
                    UserId = user4.Id,
                    Date = DateTime.Now.AddDays(-4),
                    Summary = "Valamiféle filmről szoló dolog akar ez lenni",
                    Content = "Itten volt hol nem volt volt egyszer egy kisgyerek aki szeretett volna a beadandóhoz valami szöveget kitalálni. The end",
                    Leading = false
                },
                new Article
                {
                    Title = "Sok csoki",
                    Author = user3.Name,
                    UserId = user3.Id,
                    Date = DateTime.Now.AddDays(-3),
                    Summary = "Milyen jó a fehér csoki",
                    Content = "A fehér csoki csokitartalma sok, s fehér. Kell ennél több. A helyszíni tudosítónk szerint nem.",
                    Leading = false
                },
                new Article
                {
                    Title = "Minden jó ha a vége jó",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-6),
                    Summary = "Ez egy romantikus filmről szól",
                    Content = "A végére tök barátságos lesz az egyik a másikkal aztán összeházasodnak. Happy end. Everyone is happy",
                    Leading = false
                },
                new Article
                {
                    Title = "Öregedés ellenszere",
                    Author = user3.Name,
                    UserId = user3.Id,
                    Date = DateTime.Now.AddDays(-2),
                    Summary = "Öregedés ellenszere a drogok",
                    Content = "Legyen baleset amivel tönkremegy a lába. Kell majd szednie jó kis drogokat aztán minden rendben lesz. Legyen utána egy sorozata a saját életéreől ahogy majdnem megöl embereket aztán valami oknál fogva pedig meggyógyítja őket.",
                    Leading = false
                },
                new Article
                {
                    Title = "Egyszer volt hol nem volt",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now.AddDays(-3),
                    Summary = "Hét törpe és egy gonosz kisfiú",
                    Content = "A hét törpét kirabolta a kisfiú akik utána bosszút esküdtek. Elmentek a szomszéd várba a kunyhójuk mellett ahol egy boszorkány lakott. Megkérték átkozza meg a kisfiút. A boszi teljesítette a kérést és nyúllá változott aki aztán a lopott dolgokkal így nem tudott mit kezdeni.",
                    Leading = false
                },
                new Article
                {
                    Title = "Mikrofonok",
                    Author = user4.Name,
                    UserId = user4.Id,
                    Date = DateTime.Now.AddDays(-1),
                    Summary = "Melyik mikrofont válassza karrierjéhez",
                    Content = "Alapjáraton mindegyik mikrofon jól teljesít, ha felveszi a hangját az emberek akkor már bátran javaslom. Azon belül minél nagyobb annál jobb, hiszen tudják hogy van a mondás... minél nagyobb annál jobb",
                    Leading = false
                },
                new Article
                {
                    Title = "Title valami",
                    Author = user3.Name,
                    UserId = user3.Id,
                    Date = DateTime.Now.AddDays(-4),
                    Summary = "Valami filmről bisztos szól",
                    Content = "S ebben a filmben sok sok minden történik amit az ember már követni se tud hogy őszinte legyen. Robbanás, szerelem, háború, godzilla, űrlények, ősrobbanás, fekete lyuk, minden... egyszerűen minden",
                    Leading = false
                },
                new Article
                {
                    Title = "Out of ideas?",
                    Author = user1.Name,
                    UserId = user1.Id,
                    Date = DateTime.Now,
                    Summary = "Ötletem sincs mit írja ide",
                    Content = "Ez a beadandó táblafeltöltés lényege, ideírod az összes ötletet ami eszedbe jut, lehet a végén még befejezed időben ezt a beadandót... muhahahahahahahahahahahahaa...... the end",
                    Leading = false
                },
                new Article
                {
                    Title = "Ennyi már csak elég lesz",
                    Author = user2.Name,
                    UserId = user2.Id,
                    Date = DateTime.Now.AddDays(-1),
                    Summary = "Ez az utolsó, ugye?",
                    Content = "Remélem ez lesz az utolsó táblabejegyzés amit írnom kell, mert már tényleg nincs ötletem, csak az hogy panaszkodni :'D",
                    Leading = false
                },
                new Article
                {
                    Title = "Title5.5",
                    Author = user2.Name,
                    UserId = user2.Id,
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
                        ArticleId = defaultArticles[0].Id,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllbytes(largePath)
                    });
                }
                path = Path.Combine(imageDirectory, "two.png");
                //var largePath = Path.Combine(imageDirectory, "two_big.png");
                if (File.Exists(path) /*&& File.Exists(largePath)*/)
                {
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = defaultArticles[1].Id,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllbytes(largePath)
                    });
                }

                path = Path.Combine(imageDirectory, "third.png");
                //var largePath = Path.Combine(imageDirectory, "third_big.png");
                if (File.Exists(path) /*&& File.Exists(largePath)*/)
                {
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = defaultArticles[2].Id,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllbytes(largePath)
                    });
                }

                path = Path.Combine(imageDirectory, "four.png");
                //var largePath = Path.Combine(imageDirectory, "third_big.png");
                if (File.Exists(path) /*&& File.Exists(largePath)*/)
                {
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = defaultArticles[3].Id,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllbytes(largePath)
                    });
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = defaultArticles[2].Id,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllbytes(largePath)
                    });
                    defaultPictures.Add(new Picture
                    {
                        ArticleId = defaultArticles[1].Id,
                        Image = File.ReadAllBytes(path),
                        //ImageLarge = File.ReadAllbytes(largePath)
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
