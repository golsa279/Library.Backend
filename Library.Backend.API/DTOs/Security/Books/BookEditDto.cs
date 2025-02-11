using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Security.Books
{
    public class BookEditDto
    {
        public string? Title { get; set; }
        public string? Writer { get; set; }
        public double Price { get; set; }
    }
}