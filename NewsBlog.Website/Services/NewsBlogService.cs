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
            return _context.Articles
                .OrderByDescending(l => l.Date)
                .ToList();
        }

        public Article GetArticleById(int id)
        {
            return _context.Articles.Find(id);
        }

        public Article GetLeadingArticle()
        {
            return _context.Articles
                .Where(l => l.Leading)
                .OrderByDescending(l => l.Date)
                .FirstOrDefault();
        }

        public List<Article> GetArticlesByTitle(string searchString)
        {
            return _context.Articles
                .Where(l => l.Title.Contains(searchString))
                .OrderByDescending(l => l.Date)
                .ToList();
        }

        public List<Article> GetArticlesByDate(DateTime searchDate)
        {
            return _context.Articles
                .Where(l => l.Date == searchDate)
                .OrderByDescending(l => l.Date)
                .ToList();
        }

        public List<Article> GetArticlesByContent(string searchString)
        {
            return _context.Articles
                .Where(l => l.Content == searchString)
                .OrderByDescending(l => l.Date)
                .ToList();
        }

        public bool CreateArticle(Article article)
        {
            try
            {
                _context.Articles.Add(article);
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
                _context.Articles.Update(article);
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
            var article = _context.Articles.Find(id);
            if (article == null)
                return false;

            try
            {
                _context.Articles.Remove(article);
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

        public Picture GetPicture(int? ArticleId)
        {
            if (ArticleId == null)
            {
                return null;
            }

            return _context.Pictures
                .Where(l => l.ArticleId == ArticleId)
                .FirstOrDefault();
        }

        public List<Picture> GetPictures(int ArticleId)
        {
            return _context.Pictures
                .Where(l => l.ArticleId == ArticleId)
                .ToList();
        }

        public bool CreatePicture(Picture picture)
        {
            try
            {
                _context.Pictures.Add(picture);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public NewsBlogUpdateResult UpdatePicture(Picture picture)
        {
            try
            {
                _context.Pictures.Update(picture);
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

        public bool DeletePicture(int id)
        {
            var picture = _context.Pictures.Find(id);
            if (picture == null)
                return false;

            try
            {
                _context.Pictures.Remove(picture);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool PictureExists(int id)
        {
            return _context.Pictures.Any(e => e.Id == id);
        }
        #endregion
    }
}
