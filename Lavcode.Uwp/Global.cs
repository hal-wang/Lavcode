using Lavcode.Uwp.Helpers;
using Windows.ApplicationModel;

namespace Lavcode.Uwp
{
    public static class Global
    {
        public static string Version { get; } = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

        public static bool IsNetworked => SettingHelper.Instance.Provider != Model.Provider.Sqlite;
        public static bool IsGit => SettingHelper.Instance.Provider == Model.Provider.GitHub || SettingHelper.Instance.Provider == Model.Provider.Gitee;
    }
}
