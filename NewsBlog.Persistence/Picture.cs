using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBlog.Persistence
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Article")]
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public Byte[] Image { get; set; }

        //public Byte[] ImageLarge { get; set; }
    }
}
