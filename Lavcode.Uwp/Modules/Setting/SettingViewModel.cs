using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Shell;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Lavcode.Uwp.Modules.Setting
{
    public class SettingViewModel : ObservableObject
    {
        public bool Connected => App.Frame.CurrentSourcePageType == typeof(ShellPage);

        public bool IsAuthOpen
        {
            get => SettingHelper.Instance.IsAuthOpen;
            set => SettingHelper.Instance.IsAuthOpen = value;
        }

        public bool IsBgVisible
        {
            get => SettingHelper.Instance.IsBgVisible;
            set => SettingHelper.Instance.IsBgVisible = value;
        }

        public bool IsSianoutEnable => Connected && Global.IsNetworked;
        public bool IsCleanEnable =>
            !Connected
            && Global.IsNetworked
            && ((
                    SettingHelper.Instance.Provider == Model.Provider.GitHub
                    &&
                    !string.IsNullOrEmpty(SettingHelper.Instance.GitHubToken))
                ||
                (
                    SettingHelper.Instance.Provider == Model.Provider.Gitee
                    &&
                    !string.IsNullOrEmpty(SettingHelper.Instance.GiteeToken)
                ));
    }
}
