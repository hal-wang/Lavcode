using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Lavcode.IService;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.Modules;
using Lavcode.Uwp.Modules.Auth;
using Lavcode.Uwp.Modules.Feedback;
using Lavcode.Uwp.Modules.Notices;
using Lavcode.Uwp.Modules.PasswordCore;
using Lavcode.Uwp.Modules.Setting;
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

            RegisterBaseServices();
            RegisterViewModel();
        }

        private static void RegisterBaseServices()
        {
            SimpleIoc.Default.Register<IFolderService, Service.GitHub.FolderService>();
            SimpleIoc.Default.Register<IPasswordService, Service.GitHub.PasswordService>();
            SimpleIoc.Default.Register<IIconService, Service.GitHub.IconService>();
            SimpleIoc.Default.Register<IDelectedService, Service.GitHub.DelectedService>();
            SimpleIoc.Default.Register<IConfigService, Service.GitHub.ConfigService>();
        }

        private static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<ShellPageViewModel>();
            SimpleIoc.Default.Register<WindowsHelloAuthViewModel>();
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
                    SqliteSync.ViewModel.ViewModelLocator.Register();
                    break;
                case Model.Provider.GitHub:
                    break;
            }
        }
    }
}
