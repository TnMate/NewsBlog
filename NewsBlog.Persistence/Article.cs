﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Persistence
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }

        [Required]
        [DisplayName("Author")]
        public string UserId { get; set; }

        public User User { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set;}

        public string Summary { get; set; }

        public string  Content { get; set; }

        public Boolean Leading { get; set; }
    }
}
