﻿using System;
namespace Application.Models
{
    public class Article
    {

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string[] TagList { get; set; }

        public bool Favorited { get; set; }
        public int FavoritesCount { get; set; }
        public Profile Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
