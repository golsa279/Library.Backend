using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Backend.API.Entities.Base;

namespace Library.Backend.API.Entities
{
    public class Borrow : Things
    {
        public DateTime BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public Book? Book { get; set; }

        public int BookId { get; set; }
        public int MemberId { get; set; }

        public Member? Member { get; set; }
    }
}