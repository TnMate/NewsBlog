using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Persistence.DTOs
{
    public class ArticleDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public Boolean Leading { get; set; }
    }
}
