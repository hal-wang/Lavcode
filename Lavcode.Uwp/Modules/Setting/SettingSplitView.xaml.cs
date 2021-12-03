using GalaSoft.MvvmLight.Ioc;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using System;
using Windows.ApplicationModel.Core;

namespace Lavcode.Uwp.Modules.Setting
{
    public sealed partial class SettingSplitView : HTools.Uwp.Controls.Setting.SettingSplitView
    {
        public SettingSplitView()
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
