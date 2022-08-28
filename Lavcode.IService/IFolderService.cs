using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IFolderService : IDataService
    {
        public Task DeleteFolder(string folderId, bool record = true);
        public Task AddFolder(FolderModel folder);
        public Task UpdateFolder(FolderModel folder, bool skipIcon);
        public Task<List<FolderModel>> GetFolders();
    }
}
