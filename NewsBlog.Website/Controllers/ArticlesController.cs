using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Website.Models;
using NewsBlog.Website.Services;
namespace NewsBlog.Website.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly NewsBlogService _newsBlogService;

        public ArticlesController(NewsBlogService newsBlogService)
        {
            _newsBlogService = newsBlogService;
        }

        public IActionResult Index()
        {
            var HomeView = new HomeViewModel();
            HomeView.Articles = _newsBlogService.GetArticles();
            HomeView.LeadingArticle.Article = _newsBlogService.GetLeadingArticle();
            HomeView.LeadingArticle.Picture = _newsBlogService.GetPicture(HomeView.LeadingArticle.Article.Id);

            return View(HomeView);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var list = _newsBlogService.GetArticleById((int)id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        public IActionResult Archive()
        {
            ViewData["Message"] = "The Archive where we store the old stuff";

            return View();
        }

        public FileResult Image(int? id)
        {
            // lekérjük a megadott azonosítóval rendelkező képet
            Byte[] imageContent = _newsBlogService.GetPicture(id);

            if (imageContent == null) // amennyiben nem sikerült betölteni, egy alapértelmezett képet adunk vissza
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/png");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}