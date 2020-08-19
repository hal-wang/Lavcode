using GalaSoft.MvvmLight;
using Hubery.Lavcode.Uwp.Helpers;

namespace Hubery.Lavcode.Uwp.View.Setting
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
