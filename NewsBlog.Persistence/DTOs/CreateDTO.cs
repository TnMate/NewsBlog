using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Persistence.DTOs
{
    public class CreateDTO
    {

        [Required]
        public ArticleDTO Article { get; set; }

        [Required]
        public IEnumerable<PictureDTO> Images { get; set; }
    }
}
