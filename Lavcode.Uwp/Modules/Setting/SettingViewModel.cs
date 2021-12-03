using GalaSoft.MvvmLight;
using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;

namespace Lavcode.Uwp.Modules.Setting
{
    public class SettingViewModel : ViewModelBase
    {
        public SettingViewModel()
        {
            var arr = Enum.GetValues(typeof(Provider));
            foreach (Provider item in arr) Providers.Add(item);
        }

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

        public IList<Provider> Providers { get; } = new ObservableCollection<Provider>();

        public Provider Provider
        {
            get => SettingHelper.Instance.SettingProvider;
            set
            {
                if (SettingHelper.Instance.SettingProvider == value) return;
                SettingHelper.Instance.SettingProvider = value;
                RaisePropertyChanged();
                OnProviderChange();
            }
        }

        private async void OnProviderChange()
        {
            if (await PopupHelper.ShowDialog("重启后生效，是否立即重启？", "是否重启", "是", "否") != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            await CoreApplication.RequestRestartAsync("R");
        }
    }
}
