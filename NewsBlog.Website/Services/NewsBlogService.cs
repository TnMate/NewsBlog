using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Persistence;
using System.ComponentModel.DataAnnotations;

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

        public List<Article> GetArticles(int size = 10, int? page = null)
        {
            int test;

            if ( page < 1)
            {
                test = 1;
            }
            else
            {
                test = page ?? 1;
            }

            if (size < 0)
            {
                size = 0;
            }

            return _context.Articles
                .OrderByDescending(l => l.Date)
                .Skip(size * (test - 1))
                .Take(size)
                .ToList();
        }

        public List<Article> GetArticlesBySearch(int size = 10, int? page = null, string textString = null, string titleString = null, string dateString = null)
        {
            int test;

            textString = textString ?? "";
            titleString = titleString ?? "";
            dateString = dateString ?? "";

            if (page < 1)
            {
                test = 1;
            }
            else
            {
                test = page ?? 1;
            }

            if (size < 0)
            {
                size = 0;
            }

            if (dateString == "")
            {
                return _context.Articles
                    .Where(l => l.Content.Contains(textString) && l.Title.Contains(titleString))
                    .OrderByDescending(l => l.Date)
                    .Skip(size * (test - 1))
                    .Take(size)
                    .ToList();
            }
            else
            {
                DateTime date = DateTime.Parse(dateString);
                return _context.Articles
                    .Where(l => l.Content.Contains(textString) && l.Title.Contains(titleString)
                        && l.Date.Year == date.Year && l.Date.Month == date.Month && l.Date.Day == date.Day)
                    .OrderByDescending(l => l.Date)
                    .Skip(size * (test - 1))
                    .Take(size)
                    .ToList();
            }
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

        public bool DoesItHavePictures(int? articleId)
        {
            if (articleId == null)
            {
                return false;
            }

            return _context.Pictures
                .Any(l => l.ArticleId == articleId);
        }

        public Picture GetPicture(int? articleId, int number = 0)
        {

            if (articleId == null)
            {
                return null;
            }

            return _context.Pictures
                .Where(l => l.ArticleId == articleId)
                .Skip(number)
                .FirstOrDefault();
        }

        public List<Picture> GetPictures(int? articleId)
        {
            if (articleId == null)
            {
                return null;
            }

            return _context.Pictures
                .Where(l => l.ArticleId == articleId)
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
