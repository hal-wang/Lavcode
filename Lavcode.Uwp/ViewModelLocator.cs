using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Lavcode.IService;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules;
using Lavcode.Uwp.Modules.Feedback;
using Lavcode.Uwp.Modules.Notices;
using Lavcode.Uwp.Modules.PasswordCore;
using Lavcode.Uwp.Modules.Setting;
using Lavcode.Uwp.Modules.SqliteSync.ViewModel;
using Lavcode.Uwp.ViewModel;

namespace Lavcode.Uwp
{
    public static class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

        }

        public static void Register<T>() where T : class, IConService
        {
            SimpleIoc.Default.Register<IConService, T>();

            if (typeof(T) == typeof(Service.Sqlite.ConService))
            {
                SimpleIoc.Default.Register<IFolderService, Service.Sqlite.FolderService>();
                SimpleIoc.Default.Register<IPasswordService, Service.Sqlite.PasswordService>();
                SimpleIoc.Default.Register<IIconService, Service.Sqlite.IconService>();
                SimpleIoc.Default.Register<IDelectedService, Service.Sqlite.DelectedService>();
                SimpleIoc.Default.Register<IConfigService, Service.Sqlite.ConfigService>();
            }
            else if (typeof(T) == typeof(Service.GitHub.ConService))
            {
                SimpleIoc.Default.Register<IFolderService, Service.GitHub.FolderService>();
                SimpleIoc.Default.Register<IPasswordService, Service.GitHub.PasswordService>();
                SimpleIoc.Default.Register<IIconService, Service.GitHub.IconService>();
                SimpleIoc.Default.Register<IDelectedService, Service.GitHub.DelectedService>();
                SimpleIoc.Default.Register<IConfigService, Service.GitHub.ConfigService>();
            }

            RegisterViewModel();
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<SyncViewModel>();
            SimpleIoc.Default.Register<ShellPageViewModel>();
            SimpleIoc.Default.Register<FolderListViewModel>();
            SimpleIoc.Default.Register<PasswordDetailViewModel>();
            SimpleIoc.Default.Register<PasswordMoveToViewModel>();
            SimpleIoc.Default.Register<PasswordListViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<PasswordGeneratorViewModel>();
            SimpleIoc.Default.Register<GitInfoViewModel>();
            SimpleIoc.Default.Register<FeedbackViewModel>();
            SimpleIoc.Default.Register<FeedbackDialogViewModel>();
            SimpleIoc.Default.Register<NoticesViewModel>();

            switch (SettingHelper.Instance.Provider)
            {
                case Model.Provider.Sqlite:
                    Modules.SqliteSync.ViewModel.ViewModelLocator.Register();
                    break;
                case Model.Provider.GitHub:
                    break;
            }
        }
    }
}
