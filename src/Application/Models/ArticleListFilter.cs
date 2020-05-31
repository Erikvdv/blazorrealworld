using System;
using System.Text.Json;


namespace Application.Models
{
    public enum FeedType
    {
        Global, Private
    }

    public class ArticleListFilter : ICloneable
    {
        public FeedType FeedType { get; set; }
        public string? Tag { get; set; }
        public string? Author { get; set; }
        public string? Favorited { get; set; }
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;

        public object Clone()
        {
            var serialized = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<ArticleListFilter>(serialized);
        }
    }

};
