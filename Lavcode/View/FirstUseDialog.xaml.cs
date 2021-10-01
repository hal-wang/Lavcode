using HTools.Uwp.Helpers;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.View
{
    public sealed partial class FirstUseDialog : ContentDialog
    {
        public FirstUseDialog()
        {
            InitializeComponent();

            Closing += FirstUseDialog_Closing;
        }

        private async void FirstUseDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (!IsPpChecked)
            {
                args.Cancel = true;
                await PopupHelper.ShowTeachingTipAsync(PpCheckBox, "未勾选隐私政策", "请查看并同意隐私政策", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.LeftBottom);
            }
        }

        public bool IsPpChecked
        {
            get => (bool)GetValue(IsPpCheckedProperty);
            set => SetValue(IsPpCheckedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsPpChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPpCheckedProperty =
            DependencyProperty.Register("IsPpChecked", typeof(bool), typeof(FirstUseDialog), new PropertyMetadata(false));

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
