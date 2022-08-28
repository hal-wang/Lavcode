using Lavcode.IService;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules;
using Lavcode.Uwp.Modules.Feedback;
using Lavcode.Uwp.Modules.Notices;
using Lavcode.Uwp.Modules.PasswordCore;
using Lavcode.Uwp.Modules.Setting;
using Lavcode.Uwp.Modules.SqliteSync;
using Lavcode.Uwp.Modules.SqliteSync.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lavcode.Uwp
{
    public static class ServiceProvider
    {
        public static IServiceProvider Services { get; private set; }

        public static void Register<T>() where T : class, IConService
        {
            var services = new ServiceCollection();

            ExitHandler.Instance.Register();
            RegisterSimple(services);

            services.AddSingleton<IConService, T>();

            if (typeof(Service.Sqlite.ConService) == typeof(T))
            {
                services.AddSingleton<IFolderService, Service.Sqlite.FolderService>();
                services.AddSingleton<IPasswordService, Service.Sqlite.PasswordService>();
                services.AddSingleton<IIconService, Service.Sqlite.IconService>();
                services.AddSingleton<Service.Sqlite.DelectedService, Service.Sqlite.DelectedService>();
                services.AddSingleton<SqliteFileService>();
            }
            else if (typeof(Service.BaseGit.BaseGitConService).IsAssignableFrom(typeof(T)))
            {
                services.AddSingleton<IFolderService, Service.BaseGit.FolderService>();
                services.AddSingleton<IPasswordService, Service.BaseGit.PasswordService>();
                services.AddSingleton<IIconService, Service.BaseGit.IconService>();
            }

            RegisterViewModel(services);
            Services = services.BuildServiceProvider();
        }

        public static void RegisterSimple()
        {
            var services = new ServiceCollection();
            RegisterSimple(services);
            Services = services.BuildServiceProvider();
        }

        private static void RegisterSimple(ServiceCollection services)
        {
            services.AddSingleton<NoticesViewModel>();
            services.AddSingleton<FeedbackViewModel>();
            services.AddSingleton<FeedbackDialogViewModel>();
            services.AddSingleton<GitInfoViewModel>();
            services.AddSingleton<SettingViewModel>();
        }

        private static void RegisterViewModel(ServiceCollection services)
        {
            services.AddSingleton<SyncViewModel>();
            services.AddSingleton<FolderListViewModel>();
            services.AddSingleton<PasswordDetailViewModel>();
            services.AddSingleton<PasswordMoveToViewModel>();
            services.AddSingleton<PasswordListViewModel>();
            services.AddSingleton<PasswordGeneratorViewModel>();

            switch (SettingHelper.Instance.Provider)
            {
                case Model.Provider.Sqlite:
                    Modules.SqliteSync.ViewModel.ServiceProvider.Register(services);
                    break;
                case Model.Provider.GitHub:
                    break;
            }
        }
    }
}
