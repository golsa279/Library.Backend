using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Security.librarians
{
    public class LibrarianEditDto
    {
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int Salary { get; set; }

    }
}