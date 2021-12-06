using System;
using System.IO;
using Windows.Storage;

namespace Lavcode.Uwp.Modules.SqliteSync
{
    public class SqliteFileService
    {
        /// <summary>
        /// 数据库文件名称
        /// </summary>
        public string SqliteFileName { get; } = "localDb1.lc";

        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public string SqliteFilePath =>
            FileLaunchFileName == null
            ? Path.Combine(ApplicationData.Current.LocalFolder.Path, SqliteFileName)
            : Path.Combine(ApplicationData.Current.TemporaryFolder.Path, FileLaunchFolderName, FileLaunchFileName);

        public string FileLaunchFolderName { get; } = "FileLaunch";

        private StorageFile _openedFile = null;
        public StorageFile OpenedFile
        {
            get => _openedFile;
            set
            {
                _openedFile = value;
                OnOpenedFileChange?.Invoke();
            }
        }

        public event Action OnOpenedFileChange;

        public string FileLaunchFileName { get; set; } = null;
    }
}
