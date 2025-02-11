using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Backend.API.DB;
using Library.Backend.API.DTOs.Members;
using Library.Backend.API.DTOs.Security.Generic;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.API.Controllers
{
    [ApiController]
    [Route("api/members")]
    [Authorize]
    public class MembersApiController(LibrarySystemDatabase db) : ControllerBase
    {
        private readonly LibrarySystemDatabase _db = db;

        [HttpPost("list")]
        public async Task<List<MemberSummaryDto>> List(ListRequestDto listRequestDto)
        {
            var query = _db.Members.AsQueryable();
            query = query.OrderByDescending(m => m.Id);
            if (!string.IsNullOrEmpty(listRequestDto.Keyword))
            {
                query = query.Where(m =>
                    m.Fullname.Contains(listRequestDto.Keyword) ||
                    m.Address.Contains(listRequestDto.Keyword)
                );
            }
            query = query.Skip(listRequestDto.Size * listRequestDto.Page);
            query = query.Take(listRequestDto.Size);
            Thread.Sleep(1000);
            return await
                query
                .Select(b => new MemberSummaryDto
                {
                    Id = b.Id,
                    Fullname = b.Fullname,
                    Address = b.Address,
                    Phone = b.Phone
                })
                .ToListAsync();
        }

        [HttpPost("add")]

        public async Task Add(MemberAddDto dto)
        {
            var book = new Member
            {
                Fullname = dto.Fullname,
                Address = dto.Address,
                Phone = dto.Phone
            };
            await _db.Members.AddAsync(book);
            await _db.SaveChangesAsync();
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<MemberDetailsDto>> Details(int id)
        {
            var book = await _db.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (book != null)
            {
                return new MemberDetailsDto
                {
                    Fullname = book.Fullname,
                    Address = book.Address,
                    Phone = book.Phone
                };
            }
            return NotFound();
        }
        [HttpPut("edit/{id}")]
        public async Task Edit(int id, MemberEditDto dto)
        {
            var entity = await _db.Members.FindAsync(id);
            if (entity != null)
            {
                entity.Fullname = dto.Fullname;
                entity.Address = dto.Address;
                entity.Phone = dto.Phone;
                await _db.SaveChangesAsync();
            }
        }

        [HttpPut("remove/{id}")]
        public async Task Remove(int id)
        {
            var entity = await _db.Members.FindAsync(id);
            if (entity != null)
            {
                _db.Members.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}