using Hubery.Tools.Uwp.Controls.Dialog;
using Hubery.Tools.Uwp.Helpers;
using System;
using Windows.System;
using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.View
{
    public sealed partial class FirstUseDialog : LayoutDialog
    {
        public FirstUseDialog()
        {
            this.InitializeComponent();
        }

        public bool IsPpChecked
        {
            get { return (bool)GetValue(IsPpCheckedProperty); }
            set { SetValue(IsPpCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPpChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPpCheckedProperty =
            DependencyProperty.Register("IsPpChecked", typeof(bool), typeof(FirstUseDialog), new PropertyMetadata(false));

        private async void LayoutDialog_PrimaryButtonClick(LayoutDialog sender, LayoutDialogButtonClickEventArgs args)
        {
            if (!IsPpChecked)
            {
                args.Cancel = true;
                await PopupHelper.ShowTeachingTip(PpCheckBox, "未勾选隐私政策", "请查看并同意隐私政策", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.LeftBottom);
                return;
            }
        }

        private async void Pp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Launcher.LaunchUriAsync(new Uri(Global.PpUrl));
                if (!result)
                {
                    MessageHelper.ShowDanger($"打开失败,请使用浏览器打开\n{Global.PpUrl}", 0);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
