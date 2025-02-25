using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.DTOs
{
    public class LoginDTO
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

    }

    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }   
}
