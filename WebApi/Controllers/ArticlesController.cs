using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Persistence;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Articles")]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly NewsBlogContext _context;

        public ArticlesController(NewsBlogContext context)
        {
            _context = context;
        }

        // GET: api/Articles/
        [HttpGet]
        public IEnumerable<Article> GetArticles()
        {
            return _context.Articles;
        }
        /*
        // GET: api/Articles/?articleId=5
        [HttpGet]
        public Article GetArticle([FromQuery] int articleId)
        {
            return _context.Articles.Where(i => i.Id == articleId).FirstOrDefault();
        }*/


    }
}