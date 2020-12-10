using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp.View.Sync.SyncHelper
{
    internal interface ISyncHelper
    {
        bool IsAutoVerified { get; set; }
        Task<StorageFile> GetFile();
        Task<bool> SetFile(StorageFile source, Action<StorageFile> repickFileCallback = null);
        Task<StorageFile> GetTempLocalFile();
        Task ReplaceDbFile(StorageFile storageFile);
    }
}
