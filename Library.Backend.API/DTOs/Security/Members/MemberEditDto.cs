using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Members
{
    public class MemberEditDto
    {
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}