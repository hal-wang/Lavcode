using GalaSoft.MvvmLight;
using System;

namespace Lavcode.Uwp.SqliteSync.ViewModel
{
    public class SyncHistoryItem : ObservableObject
    {
        public string FileName { get; set; }
        public DateTime LastEditTime { get; set; }
        public int PasswordCount { get; set; }
        public int FolderCount { get; set; }
        public double Size { get; set; }
    }
}
