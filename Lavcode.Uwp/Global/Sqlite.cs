using System.IO;
using Windows.Storage;

namespace Lavcode.Uwp
{
    /*
     * 数据库相关
     */
    public static partial class Global
    {
        /// <summary>
        /// 数据库文件名称
        /// </summary>
        public static string SqliteFileName { get; } = "localDb1.lc";

        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public static string SqliteFilePath =>
            FileLaunchFileName == null
            ? Path.Combine(ApplicationData.Current.LocalFolder.Path, SqliteFileName)
            : Path.Combine(ApplicationData.Current.TemporaryFolder.Path, FileLaunchFolderName, FileLaunchFileName);

        public static string FileLaunchFolderName { get; } = "FileLaunch";

        public static string FileLaunchFileName { get; set; } = null;
    }
}
