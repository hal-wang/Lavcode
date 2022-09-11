using Lavcode.Asp.Dtos;
using Lavcode.Asp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lavcode.Asp.Controllers
{
    [ApiController]
    [Route("folder")]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public FolderController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Get all folders
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetFolders()
        {
            var folders = await _databaseContext.Folders
                .Include(f => f.Icon)
                .OrderBy(f => f.Order)
                .ToArrayAsync();
            return Ok(folders);
        }

        /// <summary>
        /// Create folder
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto dto)
        {
            var order = await _databaseContext.Folders.OrderByDescending(f => f.Order).Select(f => f.Order).FirstOrDefaultAsync();
            var newFolder = await _databaseContext.Folders.AddAsync(new FolderEntity()
            {
                Id = Guid.NewGuid().ToString(),
                Icon = new IconEntity()
                {
                    IconType = dto.Icon.IconType,
                    Value = dto.Icon.Value,
                    Id = Guid.NewGuid().ToString()
                },
                Name = dto.Name,
                Order = order += 1,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            });
            await _databaseContext.SaveChangesAsync();

            return Ok(newFolder.Entity);
        }

        /// <summary>
        /// Update folder
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [HttpPut("{folderId}")]
        public async Task<IActionResult> UpdateFolder([FromBody] UpdateFolderDto dto, [FromRoute] string folderId)
        {
            await _databaseContext.Folders.Where(f => f.Id == folderId).UpdateFromQueryAsync((f) => new FolderEntity()
            {
                Name = dto.Name,
                Order = dto.Order,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
            if (dto.Icon != null)
            {
                await _databaseContext.Icons.Where(icon => icon.Id == folderId).UpdateFromQueryAsync(icon => new IconEntity()
                {
                    IconType = dto.Icon.IconType,
                    Value = dto.Icon.Value,
                });
            }
            await _databaseContext.SaveChangesAsync();

            var newFolder = await _databaseContext.Folders.Where(f => f.Id == folderId).Include(f => f.Icon).ToArrayAsync();
            return Ok(newFolder);
        }

        /// <summary>
        /// Delete a folder
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [HttpDelete("{folderId}")]
        public async Task<IActionResult> DeleteFolder([FromRoute] string folderId)
        {
            await _databaseContext.Icons
                .Where(icon => _databaseContext.Passwords.Any(p => p.FolderId == folderId && p.Id == icon.Id))
                .DeleteFromQueryAsync();
            await _databaseContext.KeyValuePairs
                .Where(kvp => kvp.Password.FolderId == folderId)
                .DeleteFromQueryAsync();
            await _databaseContext.Icons.Where(icon => icon.Id == folderId).DeleteFromQueryAsync();
            await _databaseContext.Passwords.Where(p => p.FolderId == folderId).DeleteFromQueryAsync();
            await _databaseContext.Folders.Where(f => f.Id == folderId).DeleteFromQueryAsync();
            await _databaseContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
