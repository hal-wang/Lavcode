using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Lavcode.Uwp.Modules.SqliteSync.ViewModel
{
    public class SyncHistoryItem : ObservableObject
    {
        public string FileName { get; set; }
        public int PasswordCount { get; set; }
        public int FolderCount { get; set; }
        public double Size { get; set; }
    }
}
