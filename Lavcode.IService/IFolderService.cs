using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IFolderService : IDataService
    {
        public Task DeleteFolder(string folderId, bool record = true);
        public Task AddFolder(Folder folder, Icon icon);
        public Task UpdateFolder(Folder folder, Icon icon = null);
        public Task<List<Folder>> GetFolders();
    }
}
