using System.Runtime.Serialization;

namespace Application.Models
{
    public class ArticleListFilter
    {
        public string? Tag { get; set; }
        public string? Author { get; set; }
        public string? Favorited { get; set; }
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
    }

};
