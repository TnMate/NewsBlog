﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBlog.Persistence
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Article")]
        public Article ArticleId { get; set; }

        public byte[] ImageSmall { get; set; }

        public byte[] ImageLarge { get; set; }
    }
}
