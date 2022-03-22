using System;
using System.Collections.Generic;

namespace OAuthApp.Models
{
    public partial class AuthUserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
