using GalaSoft.MvvmLight;
using Lavcode.Uwp.Helpers;

namespace Lavcode.Uwp.View.Setting
{
    class SettingViewModel : ViewModelBase
    {
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
    }
}
