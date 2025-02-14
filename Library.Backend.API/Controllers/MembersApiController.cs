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
            var member = new Member
            {
                Fullname = dto.Fullname,
                Address = dto.Address,
                Phone = dto.Phone
            };
            await _db.Members.AddAsync(member);
            await _db.SaveChangesAsync();
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<MemberDetailsDto>> Details(int id)
        {
            var member = await _db.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member != null)
            {
                return new MemberDetailsDto
                {
                    Fullname = member.Fullname,
                    Address = member.Address,
                    Phone = member.Phone
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

        [HttpDelete("remove/{id}")]
        public async Task<bool> Remove(int id)
        {
            var entity = await _db.Members.FindAsync(id);
            if (entity != null)
            {
                _db.Members.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}