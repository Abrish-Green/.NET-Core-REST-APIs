using System;
using System.Collections.Generic;

namespace OAuthApp.Models
{
    public partial class AuthUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Islogged { get; set; }
    }
}
