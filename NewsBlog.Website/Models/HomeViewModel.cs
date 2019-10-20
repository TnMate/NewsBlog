using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsBlog.Persistence;

namespace NewsBlog.Website.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Article> Articles;
        public ArticleViewModel LeadingArticle;
    }
}
