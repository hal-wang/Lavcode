using System;
using Windows.Storage;

namespace Lavcode.Uwp
{
    public static partial class Global
    {
        public static StorageFile OpenedFile { get; set; } = null;
        /// <summary>
        /// 编辑备份文件时，未保存退出确认对话框（目前在显示编辑密码未保存提示框后，弹出次未保存对话框）
        /// </summary>
        public static Action UnsaveDialogAction { get; set; }
    }
}
