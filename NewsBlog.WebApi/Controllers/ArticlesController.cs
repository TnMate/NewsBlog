using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Persistence;

namespace NewsBlog.WebApi.Controllers
{
    [Route("api/Articles")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly NewsBlogContext _context;

        public ArticlesController(NewsBlogContext context)
        {
            _context = context;
        }

        // GET: api/Items/?articleId=5
        [HttpGet]
        public IEnumerable<Article> GetItems([FromQuery] int articleId)
        {
            return _context.Articles.Where(i => i.Id == articleId);
        }


    }
}