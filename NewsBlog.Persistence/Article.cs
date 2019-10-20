using System;
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

        [DataType(DataType.Date)]
        public DateTime Date { get; set;}

        public string Summary { get; set; }

        public string  Content { get; set; }

        public Boolean Leading { get; set; }
    }
}
