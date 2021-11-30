using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Lavcode.IService;
using Lavcode.Uwp.Common;

namespace Lavcode.Uwp.ViewModel
{
    public static class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

        }

        public static async System.Threading.Tasks.Task RegisterSqlite(string fielPath)
        {
            SimpleIoc.Default.Register<IConService, Service.Sqlite.ConService>();
            await SimpleIoc.Default.GetInstance<IConService>()?.Connect(new { FilePath = fielPath });

            SimpleIoc.Default.Register<IFolderService, Service.Sqlite.FolderService>();
            SimpleIoc.Default.Register<IPasswordService, Service.Sqlite.PasswordService>();
            SimpleIoc.Default.Register<IIconService, Service.Sqlite.IconService>();
            SimpleIoc.Default.Register<IDelectedService, Service.Sqlite.DelectedService>();
            SimpleIoc.Default.Register<IConfigService, Service.Sqlite.ConfigService>();

            RegisterViewModel();
        }

        public static async System.Threading.Tasks.Task RegisterGitHub()
        {
            SimpleIoc.Default.Register<IConService, Service.GitHub.ConService>();
            await SimpleIoc.Default.GetInstance<IConService>()?.Connect(new { });

            SimpleIoc.Default.Register<IFolderService, Service.GitHub.FolderService>();
            SimpleIoc.Default.Register<IPasswordService, Service.GitHub.PasswordService>();
            SimpleIoc.Default.Register<IIconService, Service.GitHub.IconService>();
            SimpleIoc.Default.Register<IDelectedService, Service.GitHub.DelectedService>();
            SimpleIoc.Default.Register<IConfigService, Service.GitHub.ConfigService>();

            RegisterViewModel();
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<WindowsHelloAuthViewModel>();
            SimpleIoc.Default.Register<FolderListViewModel>();
            SimpleIoc.Default.Register<PasswordDetailViewModel>();
            SimpleIoc.Default.Register<PasswordMoveToViewModel>();
            SimpleIoc.Default.Register<PasswordListViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();

            switch (SettingHelper.Instance.Provider)
            {
                case Model.Provider.Sqlite:
                    SqliteSync.ViewModel.ViewModelLocator.Register();
                    break;
                case Model.Provider.GitHub:
                    break;
            }
        }
    }
}
