using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace OAuthApp.Controllers
{
    
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GreetingController : ControllerBase
    {
        [HttpGet]
        public string SayHello()
        {
            var ident = User.Identity as ClaimsIdentity;
            var username = ident.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return "Hello OAuth2 authentication. Username: " + username;
        }
        [Authorize(Roles="Customer,Admin")]
        [HttpGet]
        public string SayAll()
        {
            var ident = User.Identity as ClaimsIdentity;
            var username = ident.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return "All roles. Hello OAuth2 authentication. Username: " + username;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public string SayAdmin()
        {
            var ident = User.Identity as ClaimsIdentity;
            var username = ident.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return "Admin only. Hello OAuth2 authentication. Username: " + username;
        }
    }
}