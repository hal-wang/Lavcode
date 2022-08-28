using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IFolderService : IDataService
    {
        public Task DeleteFolder(string folderId, bool record = true);
        public Task AddFolder(FolderModel folder, IconModel icon);
        public Task UpdateFolder(FolderModel folder, IconModel icon = null);
        public Task<List<FolderModel>> GetFolders();
    }
}
