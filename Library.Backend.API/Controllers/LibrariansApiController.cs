using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Library.Backend.API.DB;
using Library.Backend.API.DTOs.Security.Generic;
using Library.Backend.API.DTOs.Security.librarians;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.API.Controllers
{
    [ApiController]
    [Route("api/librarians")]
    [Authorize]
    public class LibrariansApiController(LibrarySystemDatabase db) : ControllerBase
    {
        private readonly LibrarySystemDatabase _db = db;

        [HttpPost("list")]
        public async Task<List<LibrarianSummaryDto>> List(ListRequestDto listRequestDto)
        {
            var query = _db.Librarians.AsQueryable();
            query = query.OrderByDescending(l => l.Id);
            if (!string.IsNullOrEmpty(listRequestDto.Keyword))
            {
                query = query.Where(l =>
                    l.Fullname.Contains(listRequestDto.Keyword) ||
                    l.Address.Contains(listRequestDto.Keyword)
                );
            }
            query = query.Skip(listRequestDto.Size * listRequestDto.Page);
            query = query.Take(listRequestDto.Size);
            Thread.Sleep(1000);
            return await
                query
                .Select(b => new LibrarianSummaryDto
                {
                    Id = b.Id,
                    Fullname = b.Fullname,
                    Address = b.Address,
                    Phone = b.Phone,
                    Salary = b.Salary
                })
                .ToListAsync();
        }

        [HttpPost("add")]

        public async Task Add(LibrarianAddDto dto)
        {
            var librarian = new Librarian
            {
                Fullname = dto.Fullname,
                Address = dto.Address,
                Phone = dto.Phone,
                Salary = dto.Salary
            };
            await _db.Librarians.AddAsync(librarian);
            await _db.SaveChangesAsync();
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<LibrarianDetailsDto>> Details(int id)
        {
            var librarian = await _db.Librarians.FirstOrDefaultAsync(m => m.Id == id);
            if (librarian != null)
            {
                return new LibrarianDetailsDto
                {
                    Fullname = librarian.Fullname,
                    Address = librarian.Address,
                    Phone = librarian.Phone,
                    Salary = librarian.Salary
                };
            }
            return NotFound();
        }
        [HttpPut("edit/{id}")]
        public async Task Edit(int id, LibrarianEditDto dto)
        {
            var entity = await _db.Librarians.FindAsync(id);
            if (entity != null)
            {
                entity.Fullname = dto.Fullname;
                entity.Address = dto.Address;
                entity.Phone = dto.Phone;
                entity.Salary = dto.Salary;
                await _db.SaveChangesAsync();
            }
        }

        [HttpDelete("remove/{id}")]
        public async Task<bool> Remove(int id)
        {
            var entity = await _db.Librarians.FindAsync(id);
            if (entity != null)
            {
                _db.Librarians.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}