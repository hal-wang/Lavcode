using System.IO;
using Windows.Storage;

namespace Lavcode
{
    /*
     * 数据库相关
     */
    public static partial class Global
    {
        /// <summary>
        /// 数据库文件名称
        /// </summary>
        public static string DbFileName { get; } = "localDb1.lc";

        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public static string DbFilePath =>
            FileLaunchFileName == null
            ? Path.Combine(ApplicationData.Current.LocalFolder.Path, DbFileName)
            : Path.Combine(ApplicationData.Current.TemporaryFolder.Path, FileLaunchFolderName, FileLaunchFileName);

        public static string FileLaunchFolderName { get; } = "FileLaunch";

        public static string FileLaunchFileName { get; set; } = null;
    }
}
