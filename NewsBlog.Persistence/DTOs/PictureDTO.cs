using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NewsBlog.Persistence.DTOs
{
    public class PictureDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
