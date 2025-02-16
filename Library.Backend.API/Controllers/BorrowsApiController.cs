using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Library.Backend.API.DB;
using Library.Backend.API.DTOs.Security.Books;
using Library.Backend.API.DTOs.Security.Borrows;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Backend.API.Controllers
{
    [ApiController]
    [Route("api/borrows")]
    [Authorize]
    
    public class BorrowsApiController : ControllerBase
    {
        private readonly LibrarySystemDatabase _db;

        public BorrowsApiController(LibrarySystemDatabase db)
        {
            _db = db;
        }

        [HttpPost("list")]
        public async Task<List<Borrow>> List()
        {

            return await _db
                .Borrows
                .Include(m=>m.Book)
                .Include(b=>b.Member)
                .Where(m=>m.ReturnTime==null)
                .ToListAsync();
        }

         
        [HttpGet("details/{id}")]

        public async Task<ActionResult<BorrowDetailsDto>> Details(int id)
        {
            var borrow=await _db.Borrows.FirstOrDefaultAsync(m=>m.Id==id);
            if(borrow!=null)
            {
                return new BorrowDetailsDto
                {
                    BookId=borrow.BookId,
                    MemberId=borrow.MemberId
                };
            }
            return NotFound();
        }

        [HttpPost("add")]

        public async Task Add(BorrowAddDto dto)
        {
            var borrow=new Borrow
            {
                BookId=dto.BookId,
                MemberId=dto.MemberId,
                BorrowTime=DateTime.Now
            };
            await _db.Borrows.AddAsync(borrow);
            await _db.SaveChangesAsync();
        }

        [HttpPut("edit/{id}")]

        public async Task Edit(int id, Borrow borrow)
        {
            var entity = await _db.Borrows.FindAsync(id);
            if (entity != null)
            {
                entity.ReturnTime=DateTime.Now;
                await _db.SaveChangesAsync();
            }
        }

        [HttpDelete("remove/{id}")]

        public async Task<bool> Remove(int id)
        {
            var entity = await _db.Borrows.FindAsync(id);
            if (entity != null)
            {
                _db.Borrows.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
           
        }
        
    }
}