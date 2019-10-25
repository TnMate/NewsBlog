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

            if (HomeView.LeadingArticleView.Article == null)
            {
                return NotFound();
            }

            HomeView.LeadingArticleView.Picture = _newsBlogService.GetPicture(_newsBlogService.GetLeadingArticle().Id);
            HomeView.LeadingArticleView.PicExist = _newsBlogService.DoesItHavePictures(_newsBlogService.GetLeadingArticle().Id);

            return View(HomeView);
        }

        public IActionResult Details(int? id)
        {
            var ArticleView = new ArticleViewModel();

            if (id == null)
            {
                return BadRequest();
            }

            ArticleView.Article = _newsBlogService.GetArticleById((int)id);
            ArticleView.Picture = _newsBlogService.GetPicture((int)id);
            ArticleView.PicExist = _newsBlogService.DoesItHavePictures((int)id);


            if (ArticleView.Article == null)
            {
                return NotFound();
            }

            return View(ArticleView);
        }

        public IActionResult Gallery(int? id, int number = 0)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (number < 0)
            {
                number = 0;
            }


            ViewBag.Max = _newsBlogService.GetPictures((int)id).Count()-1;

            if (number > ViewBag.Max)
            {
                number = ViewBag.Max;
            }

            ViewBag.Number = number;

            return View(_newsBlogService.GetArticleById((int)id));
        }

        public IActionResult Archive(int? page, string textString = null, string titleString = null, string dateString = null)
        {
            var test = page ?? 1;
            var ArchiveView = new ArchiveViewModel();
            ArchiveView.Page = test;
            ViewBag.TitleString = titleString;
            ViewBag.TextString = textString;
            ViewBag.DateString = dateString;
            if (textString != null || titleString != null || dateString != null)
            {
                ArchiveView.Articles = _newsBlogService.GetArticlesBySearch(20, test, textString, titleString, dateString);
            }
            else
            {
                ArchiveView.Articles = _newsBlogService.GetArticles(20 , test);
            }

            return View(ArchiveView);
        }

        public FileResult PictureForArticle(int? pictureId, int number = 0)
        {
            Picture picture = _newsBlogService.GetPicture(pictureId, number);

            if (picture == null) 
                return File("~/images/NoImage.png", "image/png");

            return File(picture.Image, "image/png");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}