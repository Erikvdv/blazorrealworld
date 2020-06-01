using System;
using System.Collections.Generic;
using Application.Models;

namespace Infrastructure.Clients.Models
{
    public class GetTagsResponse
    {
        public string[]? Tags { get; set; }
    }

    public class LoginRequest
    {
        public Login? User { get; set; }
    }

    public class LoginResponse
    {
        public User? User { get; set; }
    }

    public class ArticleObject
    {
        public Article? Article { get; set; }
    }

    public class NewArticleRequest
    {
        public NewArticle? Article { get; set; }
    }

    public class CommentsResponse
    {
        public List<Comment>? Comments { get; set; }
    }

    public class ProfileResponse
    {
        public Profile? Profile { get; set; }
    }

    public class UserResponse
    {
        public User? User { get; set; }
    }

    public class UserUpdateRequest
    {
        public User? User { get; set; }
    }

    public class ErrorResponse
    {
        public Dictionary<string, string[]>? Errors { get; set; }
    }

}
