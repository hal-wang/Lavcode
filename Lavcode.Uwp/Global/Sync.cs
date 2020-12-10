using Hubery.Lavcode.Uwp.Helpers;
using Hubery.Lavcode.Uwp.View.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp
{
    /*
     * 同步相关
     */
    public static partial class Global
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

        public static IReadOnlyList<CloudItem> CloudItems => new List<CloudItem>()
        {
            new CloudItem(CloudType.Jgy,"坚果云","https://dav.jianguoyun.com/dav"),
            new CloudItem(CloudType.Box,"Box","https://www.box.com/dav"),
            new CloudItem(CloudType.Yandex,"Yandex","https://webdav.yandex.com"),
            new CloudItem(CloudType._4Shared,"4Shared","https://webdav.4shared.com"),
            new CloudItem(CloudType.Other,"其他",SettingHelper.Instance.DavCustomUrl),
        };

        /// <summary>
        /// DAV Base URL
        /// </summary>
        public static string RemoteBaseUrl => CloudItems.Where(item => item.CloudType == SettingHelper.Instance.DavCloudType).Select(item => item.Url).FirstOrDefault();

        /// <summary>
        /// 云盘中，同步使用的相对根目录
        /// </summary>
        public static string RemoteRootFolder { get; } = "Lavcode";

        /// <summary>
        /// 云盘中的数据库文件名称
        /// </summary>
        public static string RemoteFileName { get; } = "sync1.lc";

        /// <summary>
        /// 编辑备份文件时，未保存退出确认对话框（目前在显示编辑密码未保存提示框后，弹出次未保存对话框）
        /// </summary>
        public static Action UnsaveDialogAction { get; set; }

        public static StorageFile OpenedFile { get; set; } = null;
    }
}
