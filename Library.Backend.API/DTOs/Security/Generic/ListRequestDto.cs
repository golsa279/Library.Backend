using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Security.Generic
{
    public class ListRequestDto
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string? Keyword { get; set; }
        public string? Type { get; set; }
    }
}