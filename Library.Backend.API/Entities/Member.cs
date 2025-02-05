using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Backend.API.Entities.Base;

namespace Library.Backend.API.Entities
{
    public class Member : Things
    {
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}