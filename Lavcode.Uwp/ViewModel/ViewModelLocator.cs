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

        public static async System.Threading.Tasks.Task Register()
        {
            await RegisterData();
            RegisterViewModel();
        }

        private static async System.Threading.Tasks.Task RegisterData()
        {
            switch (SettingHelper.Instance.Provider)
            {
                case Model.Provider.Sqlite:
                    SimpleIoc.Default.Register<IConService, Service.Sqlite.ConService>();

                    SimpleIoc.Default.Register<IFolderService, Service.Sqlite.FolderService>();
                    SimpleIoc.Default.Register<IPasswordService, Service.Sqlite.PasswordService>();
                    SimpleIoc.Default.Register<IKeyValuePairService, Service.Sqlite.KeyValuePairService>();
                    SimpleIoc.Default.Register<IIconService, Service.Sqlite.IconService>();
                    SimpleIoc.Default.Register<IDelectedService, Service.Sqlite.DelectedService>();
                    SimpleIoc.Default.Register<IConfigService, Service.Sqlite.ConfigService>();
                    break;
                case Model.Provider.GitHub:
                    //SimpleIoc.Default.Register<IFolderService, Service.GitHub.FolderService>();
                    //SimpleIoc.Default.Register<IPasswordService, Service.GitHub.PasswordService>();
                    //SimpleIoc.Default.Register<IKeyValuePairService, Service.GitHub.KeyValuePairService>();
                    //SimpleIoc.Default.Register<IIconService, Service.GitHub.IconService>();
                    SimpleIoc.Default.Register<IDelectedService, Service.GitHub.DelectedService>();
                    SimpleIoc.Default.Register<IConfigService, Service.GitHub.ConfigService>();
                    break;
                case Model.Provider.Gitee:
                    break;
            }
            await SimpleIoc.Default.GetInstance<IConService>()?.Connect(new { FilePath = Global.SqliteFilePath });
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AuthViewModel>();
            SimpleIoc.Default.Register<FolderListViewModel>();
            SimpleIoc.Default.Register<PasswordDetailViewModel>();
            SimpleIoc.Default.Register<PasswordMoveToViewModel>();
            SimpleIoc.Default.Register<PasswordListViewModel>();

            switch (SettingHelper.Instance.Provider)
            {
                case Model.Provider.Sqlite:
                    SqliteSync.ViewModel.ViewModelLocator.Register();
                    break;
                case Model.Provider.GitHub:
                    break;
                case Model.Provider.Gitee:
                    break;
            }
        }
    }
}
