using System;
using System.Text;

namespace Lavcode.Uwp.Modules.SqliteSync
{
    internal class Constant
    {
        /// <summary>
        /// 同步时，临时文件存放的文件夹名称
        /// </summary>
        public static string SyncTempFolderName { get; } = "Sync";

        /// <summary>
        /// 同步时，已加密的文件
        /// </summary>
        public static string SyncTempEncryptedFileName { get; } = "eTemp";

        /// <summary>
        /// 同步时，未加密的文件
        /// </summary>
        public static string SyncTempUnencryptedFileName { get; } = "uetemp";

        /// <summary>
        /// 同步时，临时拷贝的本地文件
        /// </summary>
        public static string SyncTempLocalFileName { get; } = "tempLocal";

        /// <summary>
        /// 加密文件头
        /// </summary>
        public static string SyncFileHeader { get; } = "LAVCODE1";

        /// <summary>
        /// SHA-2 字节长度
        /// </summary>
        public static int Sha2ByteLength { get; } = 64;

        /// <summary>
        /// 加密文件头长度，包含识别部分和密码
        /// </summary>
        public static int SyncFileHeaderLength { get; } = SyncFileHeader.Length + Sha2ByteLength;

        /// <summary>
        /// 同步历史文件夹
        /// </summary>
        public static string SyncHistoryFolder { get; } = "History";

        /// <summary>
        /// AES加密使用的偏移量
        /// </summary>
        public static byte[] AesIv { get; } = Encoding.UTF8.GetBytes("Lavcode,the gift");

        /// <summary>
        /// 云盘中，同步使用的相对根目录
        /// </summary>
        public static string RemoteRootFolder { get; } = "Lavcode";

        /// <summary>
        /// 云盘中的数据库文件名称
        /// </summary>
        public static string RemoteFileName { get; } = "sync1.lc";
    }
}
