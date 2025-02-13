using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Backend.API.DB;
using Library.Backend.API.DTOs.Security;
using Library.Backend.API.DTOs.Security.Books;
using Library.Backend.API.DTOs.Security.Generic;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.API.Controllers
{
    [ApiController]
    [Route("api/books")]

    public class BooksApiController(LibrarySystemDatabase db) : ControllerBase
    {
        private readonly LibrarySystemDatabase _db = db;

        [HttpPost("list")]
        public async Task<List<BookSummaryDto>> List(ListRequestDto listRequestDto)
        {
            var query = _db.Books.AsQueryable();
            query = query.OrderByDescending(m => m.Id);
            if (!string.IsNullOrEmpty(listRequestDto.Keyword))
            {
                query=query.Where(m=>
                    m.Title.Contains(listRequestDto.Keyword) ||
                    m.Writer.Contains(listRequestDto.Keyword)
                );
            }
            query=query.Skip(listRequestDto.Size*listRequestDto.Page);
            query=query.Take(listRequestDto.Size);
            Thread.Sleep(1000);
            return await
                query
                .Select(b=>new BookSummaryDto
                {
                    Id=b.Id,
                    Price=b.Price,
                    Title=b.Title,
                    Writer=b.Writer
                })
                .ToListAsync();
        }

        [HttpPost("add")]
        [Authorize]

        public async Task Add(BookAddDto dto)
        {
            var book=new Book
            {
                Title=dto.Title,
                Writer=dto.Writer,
                Price=dto.Price
            };
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();
        }
        
        [HttpGet("details/{id}")]
        [Authorize]
        public async Task<ActionResult<BookDetailsDto>> Details(int id)
        {
            var book=await _db.Books.FirstOrDefaultAsync(m=>m.Id==id);
            if(book!=null)
            {
                return new BookDetailsDto
                {
                    Title=book.Title,
                    Writer=book.Writer,
                    Price=book.Price
                };
            }
            return NotFound();
        }
        [HttpPut("edit/{id}")]
        [Authorize]
        public async Task Edit(int id,BookEditDto dto)
        {
            var entity = await _db.Books.FindAsync(id);
            if (entity != null)
            {
                entity.Title = dto.Title;
                entity.Writer = dto.Writer;
                entity.Price = dto.Price;
                await _db.SaveChangesAsync();
            }
        }

        [HttpDelete("remove/{id}")]
        [Authorize]
        public async Task<bool> Remove(int id)
        {
            var entity = await _db.Books.FindAsync(id);
            if (entity != null)
            {
                _db.Books.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}