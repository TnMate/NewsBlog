using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Articles")]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly NewsBlogContext _context;
        private readonly UserManager<User> _userManager;

        public ArticlesController(NewsBlogContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Articles/
        [HttpGet]
        public IEnumerable<Article> GetArticles()
        {
            var userId = _userManager.GetUserId(User);
            return _context.Articles.Where(i => i.UserId == userId).OrderBy(i => i.Date);
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public IActionResult GetArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Articles.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public IActionResult PostArticle([FromBody] CreateDTO createDTO)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var test2 = _userManager.GetUserName(User);
                //var test = _context.Users.Find(userId).Name;
                var addedArticle = _context.Articles.Add(new Article
                {
                    Title = createDTO.Article.Title,
                    Author = test2,
                    UserId = userId,
                    Date = DateTime.Now,
                    Summary = createDTO.Article.Summary,
                    Content = createDTO.Article.Content,
                    Leading = createDTO.Article.Leading
                });
                
                createDTO.Article.Id = addedArticle.Entity.Id;
                foreach (var item in createDTO.Images)
                {
                    _context.Pictures.Add(new Picture
                    {
                        ArticleId = addedArticle.Entity.Id,
                        Image = item.Image
                    });
                }

                _context.SaveChanges();

                return Created(Request.GetUri() + addedArticle.Entity.Id.ToString(), createDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        //[Authorize(Roles = "admin")]
        public IActionResult PutArticle([FromBody] ArticleDTO articleDTO)
        {
            try
            {
                Article article = _context.Articles.FirstOrDefault(b => b.Id == articleDTO.Id);

                if (article == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                var userId = _userManager.GetUserId(User);

                article.Title = articleDTO.Title;
                article.Date = DateTime.Now;
                article.Summary = articleDTO.Summary;
                article.Content = articleDTO.Content;
                article.Leading = articleDTO.Leading;

                _context.Articles.Update(article);

                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public IActionResult DeleteItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Articles.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            var items = _context.Pictures.Where(i => i.ArticleId == id);
            _context.Articles.Remove(item);
            foreach (var a in items)
            {
                _context.Pictures.Remove(a);
            }
            _context.SaveChanges();

            return Ok(item);
        }
    }
}