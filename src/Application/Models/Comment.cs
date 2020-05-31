using System;
namespace Application.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public Profile Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
