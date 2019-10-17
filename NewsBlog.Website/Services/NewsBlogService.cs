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

        public enum TodoListUpdateResult
        {
            Success,
            ConcurrencyError,
            DbError
        }

        public NewsBlogService(NewsBlogContext context)
        {
            _context = context;
        }
    }
}
