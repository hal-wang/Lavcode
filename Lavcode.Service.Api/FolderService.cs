using HTools;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.Api.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.Api
{
    public class FolderService : IFolderService
    {
        private readonly ConService _cs;
        public FolderService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task AddFolder(FolderModel folder)
        {
            var newFolder = await _cs.PostAsync<GetFolderDto>("folder", new CreateFolderDto()
            {
                Name = folder.Name,
                Icon = new UpsertIconDto()
                {
                    IconType = folder.Icon.IconType,
                    Value = folder.Icon.Value
                },
            });
            var newFolderModel = newFolder.ToModel();

            folder.UpdatedAt = newFolderModel.UpdatedAt;
            folder.Order = newFolderModel.Order;
            folder.Icon = newFolderModel.Icon;
            folder.Id = newFolderModel.Id;
        }

        public async Task DeleteFolder(string folderId, bool record = true)
        {
            await _cs.DeleteAsync("folder/:folderId", param: new
            {
                folderId
            });
        }

        public async Task<List<FolderModel>> GetFolders()
        {
            var folders = await _cs.GetAsync<List<GetFolderDto>>("folder");
            return folders.Select(item => item.ToModel()).ToList();
        }

        public async Task UpdateFolder(FolderModel folder, bool skipIcon)
        {
            var newFolder = await _cs.PutAsync<GetFolderDto>("folder/:folderId", new UpdateFolderDto()
            {
                Name = folder.Name,
                Icon = skipIcon ? null : new UpsertIconDto()
                {
                    IconType = folder.Icon.IconType,
                    Value = folder.Icon.Value
                },
                Order = folder.Order,
            },
            new
            {
                folderId = folder.Id
            });
            var newFolderModel = newFolder.ToModel();

            folder.UpdatedAt = newFolderModel.UpdatedAt;
            if (!skipIcon)
            {
                folder.Icon = newFolderModel.Icon;
            }
        }
    }
}
