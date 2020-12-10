using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp.View.Sync.SyncHelper
{
    internal class FileSyncHelper : BaseSyncHelper, ISyncHelper
    {
        private FileSyncHelper() { }

        /// <summary>
        /// 本地文件导入的文件。如果是合并，则需要使用这个文件作为导出。
        /// 如果是打开文件，则此引用为打开的文件。
        /// </summary>
        private StorageFile _pickedFile = null;

        /// <summary>
        /// 通过文件初始化
        /// </summary>
        /// <param name="storageFile"></param>
        /// <returns></returns>
        public async static Task<ISyncHelper> Create(StorageFile storageFile)
        {
            var result = new FileSyncHelper();
            if (!await result.LoadFromFile(storageFile))
            {
                return null;
            }
            return result;
        }

        public static ISyncHelper Create()
        {
            return new FileSyncHelper();
        }

        private async Task<bool> LoadFromFile(StorageFile storageFile)
        {
            _pickedFile = storageFile;
            var file = await DecryptFile(_pickedFile);
            if (file == null)
            {
                return false;
            }

            var launchFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Global.FileLaunchFolderName, CreationCollisionOption.OpenIfExists);
            await file.MoveAsync(launchFolder, DateTime.Now.ToString("yyMMddHHmmss"), NameCollisionOption.GenerateUniqueName);
            Global.FileLaunchFileName = file.Name;
            return true;
        }

        /// <summary>
        /// 从文件系统获取文件
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFile> GetFile()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".lc");
            _pickedFile = await picker.PickSingleFileAsync();
            if (_pickedFile == null)
            {
                return null;
            }

            return await DecryptFile(_pickedFile);
        }

        /// <summary>
        /// 导出文件到文件系统
        /// </summary>
        /// <param name="source"></param>
        /// <param name="repickFileCallback"></param>
        /// <returns></returns>
        public async Task<bool> SetFile(StorageFile source, Action<StorageFile> repickFileCallback = null)
        {
            StorageFile pickFile = repickFileCallback == null ? _pickedFile : null;

            var encryptedFile = await EncryptFile(source);
            if (encryptedFile == null)
            {
                return false;
            }

            if (pickFile == null)
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker
                {
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop,
                    SuggestedFileName = "Lavcode备份"
                };
                savePicker.FileTypeChoices.Add("Lavcode", new List<string>() { ".lc" });
                pickFile = await savePicker.PickSaveFileAsync();
                if (pickFile == null)
                {
                    return false;
                }
            }
            _pickedFile = pickFile;

            await encryptedFile.CopyAndReplaceAsync(_pickedFile);
            repickFileCallback?.Invoke(_pickedFile);
            return true;
        }
    }
}
