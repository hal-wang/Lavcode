using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Lavcode.IService;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules;
using Lavcode.Uwp.Modules.Feedback;
using Lavcode.Uwp.Modules.Notices;
using Lavcode.Uwp.Modules.PasswordCore;
using Lavcode.Uwp.Modules.SqliteSync;
using Lavcode.Uwp.Modules.SqliteSync.ViewModel;

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
            ExitHandler.Instance.Register();
            SimpleIoc.Default.Reset();
            SimpleIoc.Default.Register<IConService, T>();

            if (typeof(Service.Sqlite.ConService) == typeof(T))
            {
                SimpleIoc.Default.Register<IFolderService, Service.Sqlite.FolderService>();
                SimpleIoc.Default.Register<IPasswordService, Service.Sqlite.PasswordService>();
                SimpleIoc.Default.Register<IIconService, Service.Sqlite.IconService>();
                SimpleIoc.Default.Register<IDelectedService, Service.Sqlite.DelectedService>();
                SimpleIoc.Default.Register<IConfigService, Service.Sqlite.ConfigService>();
                SimpleIoc.Default.Register<SqliteFileService>();
            }
            else if (typeof(Service.BaseGit.BaseGitConService).IsAssignableFrom(typeof(T)))
            {
                SimpleIoc.Default.Register<IFolderService, Service.BaseGit.FolderService>();
                SimpleIoc.Default.Register<IPasswordService, Service.BaseGit.PasswordService>();
                SimpleIoc.Default.Register<IIconService, Service.BaseGit.IconService>();
                SimpleIoc.Default.Register<IDelectedService, Service.BaseGit.DelectedService>();
                SimpleIoc.Default.Register<IConfigService, Service.BaseGit.ConfigService>();
            }

            RegisterViewModel();
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<SyncViewModel>();
            SimpleIoc.Default.Register<FolderListViewModel>();
            SimpleIoc.Default.Register<PasswordDetailViewModel>();
            SimpleIoc.Default.Register<PasswordMoveToViewModel>();
            SimpleIoc.Default.Register<PasswordListViewModel>();
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
