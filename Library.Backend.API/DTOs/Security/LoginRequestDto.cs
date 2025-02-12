using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Security
{
    public class LoginRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        
    }
}