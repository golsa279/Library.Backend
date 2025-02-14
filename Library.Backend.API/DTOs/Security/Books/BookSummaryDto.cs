using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Security.Books
{
    public class BookSummaryDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Writer { get; set; }
        public double Price { get; set; }
        public string? Category { get; set; }
        public string? ImgPath { get; set; }

    }
}