using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.API.DB
{
    public class LibrarySystemDatabase(DbContextOptions options) : IdentityDbContext<LibraryUser>(options) //DbContext
    {
        public required DbSet<Book> Books { get; set; }
        public required DbSet<Member> Members { get; set; }
        public required DbSet<Borrow> Borrows { get; set; }
    }
}