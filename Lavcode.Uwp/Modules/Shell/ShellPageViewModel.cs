using GalaSoft.MvvmLight;
using Windows.Storage;

namespace Lavcode.Uwp.ViewModel
{
    public class ShellPageViewModel : ViewModelBase
    {
        private StorageFile _openedFile = null;
        public StorageFile OpenedFile
        {
            get => _openedFile;
            set => Set(ref _openedFile, value);
        }
    }
}
