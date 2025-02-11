using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Backend.API.DTOs.Security.Borrows
{
    public class BorrowDetailsDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
    }
}