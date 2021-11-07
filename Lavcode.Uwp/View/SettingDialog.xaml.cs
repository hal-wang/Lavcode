using GalaSoft.MvvmLight.Ioc;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.ViewModel;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View
{
    public sealed partial class SettingDialog : ContentDialog
    {
        public SettingDialog()
        {
            DataContext = VM;
            this.InitializeComponent();
        }

        public SettingViewModel VM { get; } = SimpleIoc.Default.GetInstance<SettingViewModel>();

        private async void OnStorageTypeSelect(HTools.Uwp.Controls.Setting.SelectSettingCell sender, object args)
        {
            SettingHelper.Instance.Provider = (Provider)args;
            await CoreApplication.RequestRestartAsync("R");
        }
    }
}
