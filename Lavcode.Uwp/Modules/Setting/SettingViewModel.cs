using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Lavcode.IService;
using Lavcode.Uwp.Helpers;

namespace Lavcode.Uwp.Modules.Setting
{
    public class SettingViewModel : ViewModelBase
    {
        public bool Connected => SimpleIoc.Default.ContainsCreated<IConService>();

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

        public bool IsSianoutEnable => Connected && (SettingHelper.Instance.Provider == Model.Provider.GitHub || SettingHelper.Instance.Provider == Model.Provider.Gitee);
    }
}
