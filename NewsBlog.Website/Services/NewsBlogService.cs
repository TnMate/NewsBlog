using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Persistence;

namespace NewsBlog.Website.Services
{
    public class NewsBlogService
    {
        private readonly NewsBlogContext _context;

        public enum NewsBlogUpdateResult
        {
            Success,
            ConcurrencyError,
            DbError
        }

        public NewsBlogService(NewsBlogContext context)
        {
            _context = context;
        }

        #region Article

        public List<Article> GetArticles()
        {
            return _context.Articles.ToList();
        }

        public Article GetArticleById(int id)
        {
            return _context.Articles.Find(id);
        }

        public Article GetLeadingArticle()
        {
            return _context.Articles
                .Where(l => l.Leading)
                .OrderBy(l => l.Date)
                .FirstOrDefault();
        }

        public List<Article> GetArticlesByTitle(string searchString)
        {
            return _context.Articles
                .Where(l => l.Title.Contains(searchString))
                .OrderBy(l => l.Title)
                .ToList();
        }

        public List<Article> GetArticlesByDate(DateTime searchDate)
        {
            return _context.Articles
                .Where(l => l.Date == searchDate)
                .OrderBy(l => l.Date)
                .ToList();
        }

        public List<Article> GetArticlesByContent(string searchString)
        {
            return _context.Articles
                .Where(l => l.Content == searchString)
                .OrderBy(l => l.Date)
                .ToList();
        }

        public bool CreateArticle(Article article)
        {
            try
            {
                _context.Add(article);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public NewsBlogUpdateResult UpdateArticle(Article article)
        {
            try
            {
                _context.Update(article);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NewsBlogUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return NewsBlogUpdateResult.DbError;
            }

            return NewsBlogUpdateResult.Success;
        }

        public bool DeleteArticle(int id)
        {
            var list = _context.Articles.Find(id);
            if (list == null)
                return false;

            try
            {
                _context.Articles.Remove(list);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        #endregion

        #region Picture

        #endregion
    }
}
