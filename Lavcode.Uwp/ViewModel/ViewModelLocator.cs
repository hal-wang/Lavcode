using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Lavcode.IService;
using Lavcode.Uwp.Helpers;

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
                case Lavcode.Model.Provider.Sqlite:
                    SimpleIoc.Default.Register<IConService, Service.Sqlite.ConService>();
                    await SimpleIoc.Default.GetInstance<IConService>().Connect(new { FilePath = Global.DbFilePath });

                    SimpleIoc.Default.Register<IFolderService, Service.Sqlite.FolderService>();
                    SimpleIoc.Default.Register<IPasswordService, Service.Sqlite.PasswordService>();
                    SimpleIoc.Default.Register<IKeyValuePairService, Service.Sqlite.KeyValuePairService>();
                    SimpleIoc.Default.Register<IIconService, Service.Sqlite.IconService>();
                    SimpleIoc.Default.Register<IDelectedService, Service.Sqlite.DelectedService>();
                    SimpleIoc.Default.Register<IConfigService, Service.Sqlite.ConfigService>();
                    break;
                case Lavcode.Model.Provider.GitHub:
                    break;
                case Lavcode.Model.Provider.Gitee:
                    break;

            }
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AuthViewModel>();
            SimpleIoc.Default.Register<FolderListViewModel>();
            SimpleIoc.Default.Register<PasswordDetailViewModel>();
            SimpleIoc.Default.Register<PasswordMoveToViewModel>();
            SimpleIoc.Default.Register<PasswordListViewModel>();
            SimpleIoc.Default.Register<SyncHistoryViewModel>();
        }
    }
}
