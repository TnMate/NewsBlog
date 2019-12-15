using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace NewsBlog.Persistence
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
