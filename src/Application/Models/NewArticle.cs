using System.Collections.Generic;

namespace Application.Models
{
    public class NewArticle
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Body { get; set; } = "";
        public HashSet<string> TagList { get; set; } = new HashSet<string> { };
    }
}
