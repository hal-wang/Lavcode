using Lavcode.Asp.Dtos;
using Lavcode.Asp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lavcode.Asp.Controllers
{
    [ApiController]
    [Route("password")]
    [Authorize]
    public class PasswordController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public PasswordController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Get passwords
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetPasswords([FromQuery(Name = "folderId")] string folderId)
        {
            var folders = await _databaseContext.Passwords
                .Where(item => item.FolderId == folderId)
                .Include(p => p.Icon)
                .Include(p => p.KeyValuePairs)
                .OrderBy(p => p.Order)
                .ToArrayAsync();
            return Ok(folders);
        }

        /// <summary>
        /// Create password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreatePassword([FromBody] CreatePasswordDto dto)
        {
            var order = await _databaseContext.Passwords.OrderByDescending(p => p.Order).Select(p => p.Order).FirstOrDefaultAsync();
            var passwordId = Guid.NewGuid().ToString();
            var newPassword = await _databaseContext.Passwords.AddAsync(new PasswordEntity()
            {
                Id = Guid.NewGuid().ToString(),
                FolderId = dto.FolderId,
                KeyValuePairs = dto.KeyValuePairs.Select(kvp => new KeyValuePairEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    PasswordId = passwordId,
                    Key = kvp.Key,
                    Value = kvp.Value
                }).ToArray(),
                Icon = new IconEntity()
                {
                    IconType = dto.Icon.IconType,
                    Value = dto.Icon.Value,
                    Id = passwordId,
                },
                Title = dto.Title,
                Remark = dto.Remark,
                Value = dto.Value,
                Order = order += 1,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            });
            await _databaseContext.SaveChangesAsync();

            return Ok(newPassword.Entity);
        }

        /// <summary>
        /// Update password
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="passwordId"></param>
        /// <returns></returns>
        [HttpPut("{passwordId}")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto, [FromRoute] string passwordId)
        {
            await _databaseContext.Passwords.Where(p => p.Id == passwordId).UpdateFromQueryAsync((p) => new PasswordEntity()
            {
                Title = dto.Title,
                Remark = dto.Remark,
                Value = dto.Value,
                Order = dto.Order,
                FolderId = dto.FolderId,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
            if (dto.Icon != null)
            {
                await _databaseContext.Icons.Where(icon => icon.Id == passwordId).UpdateFromQueryAsync(icon => new IconEntity()
                {
                    IconType = dto.Icon.IconType,
                    Value = dto.Icon.Value,
                });
            }
            if (dto.KeyValuePairs != null)
            {
                await _databaseContext.KeyValuePairs.Where(kvp => kvp.PasswordId == passwordId).DeleteFromQueryAsync();
                await _databaseContext.KeyValuePairs.AddRangeAsync(dto.KeyValuePairs.Select(kvp => new KeyValuePairEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    PasswordId = passwordId,
                    Key = kvp.Key,
                    Value = kvp.Value
                }).ToArray());
            }
            await _databaseContext.SaveChangesAsync();

            var newFolder = await _databaseContext.Passwords
                .Where(p => p.Id == passwordId)
                .Include(p => p.Icon)
                .Include(p => p.KeyValuePairs)
                .FirstOrDefaultAsync();
            return Ok(newFolder);
        }

        /// <summary>
        /// Delete a password
        /// </summary>
        /// <param name="passwordId"></param>
        /// <returns></returns>
        [HttpDelete("{passwordId}")]
        public async Task<IActionResult> DeletePassword([FromRoute] string passwordId)
        {
            await _databaseContext.KeyValuePairs
                .Where(kvp => kvp.PasswordId == passwordId)
                .DeleteFromQueryAsync();
            await _databaseContext.Icons
                .Where(kvp => kvp.Id == passwordId)
                .DeleteFromQueryAsync();
            await _databaseContext.Passwords.Where(p => p.Id == passwordId).DeleteFromQueryAsync();
            await _databaseContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
