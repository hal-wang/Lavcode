using GalaSoft.MvvmLight;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lavcode.Uwp.ViewModel
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
    }
}
