using GalaSoft.MvvmLight;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp.View.Main
{
    public class MainViewModel : ViewModelBase
    {
        private StorageFile _openedFile = null;
        public StorageFile OpenedFile
        {
            get { return _openedFile; }
            set { Set(ref _openedFile, value); }
        }
    }
}
