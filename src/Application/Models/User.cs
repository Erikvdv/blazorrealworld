using System;
namespace Application.Models
{
    public class User
    {
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Username { get; set; }
    }
}
