using System;
using System.Collections.Generic;
using Application.Models;

namespace Infrastructure.Clients.Models
{
    public class GetTagsResponse
    {
        public string[] Tags { get; set; }
    }

    public class LoginRequest
    {
        public Login User { get; set; }
    }

    public class LoginResponse
    {
        public User User { get; set; }
    }

    public class ErrorResponse
    {
        public Dictionary<string, string[]> Errors { get; set; }
    }

}
