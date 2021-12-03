using GalaSoft.MvvmLight.Ioc;

namespace Lavcode.Uwp.Modules.SqliteSync.ViewModel
{
    public static class ViewModelLocator
    {
        public static void Register()
        {
            RegisterViewModel();
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<SyncHistoryViewModel>();
            SimpleIoc.Default.Register<SyncViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
        }
    }
}
