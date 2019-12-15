using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Persistence.DTOs
{
    public class LoginDto
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}
