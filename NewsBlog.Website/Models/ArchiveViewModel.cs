using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsBlog.Persistence;

namespace NewsBlog.Website.Models
{
    public class ArchiveViewModel
    {
        public IEnumerable<Article> Articles;
        public int Page;
    }
}
