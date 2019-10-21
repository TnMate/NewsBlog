using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Website.Models;
using NewsBlog.Website.Services;
using NewsBlog.Persistence;

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
            HomeView.LeadingArticleView = new ArticleViewModel();
            HomeView.LeadingArticleView.Article = _newsBlogService.GetLeadingArticle();
            HomeView.LeadingArticleView.Picture = _newsBlogService.GetPicture(_newsBlogService.GetLeadingArticle().Id);

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

        public IActionResult Archive(int? page)
        {
            ViewData["Message"] = "The Archive where we store the old stuff";

            return View();
        }

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}