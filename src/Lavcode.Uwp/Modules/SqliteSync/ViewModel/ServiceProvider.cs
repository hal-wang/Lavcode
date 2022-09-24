using Microsoft.Extensions.DependencyInjection;

namespace Lavcode.Uwp.Modules.SqliteSync.ViewModel
{
    public static class ServiceProvider
    {
        public static void Register(ServiceCollection services)
        {
            services.AddSingleton<SyncHistoryViewModel>();
            services.AddSingleton<SyncViewModel>();
            services.AddSingleton<LoginViewModel>(); 
        }
    }
}
