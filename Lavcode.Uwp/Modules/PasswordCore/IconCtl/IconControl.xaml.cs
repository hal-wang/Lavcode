using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore.IconCtl
{
    public sealed partial class IconControl : UserControl
    {
        public IconControl()
        {
            this.InitializeComponent();
        }

        public Icon Icon
        {
            get { return (Icon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Icon), typeof(IconControl), new PropertyMetadata(null, OnIconChanged));


        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var iconControl = d as IconControl;
            if (e.NewValue is not Icon icon)
            {
                icon = new Icon()
                {
                    IconType = IconType.SegoeMDL2,
                    Value = string.Empty
                };
            }
            iconControl.OnIconChanged(icon);
        }

        private async void OnIconChanged(Icon icon)
        {
            switch (icon.IconType)
            {
                case IconType.SegoeMDL2:
                    VM.SegoeMDL2Icon = (icon.Value != null && icon.Value.Length > 0) ? icon.Value[0].ToString() : string.Empty;
                    break;
                case IconType.Img:
                    if ((SettingHelper.Instance.Provider == Provider.Gitee || SettingHelper.Instance.Provider == Provider.GitHub)
                        && icon.Value.Length > 60000)
                    {
                        HTools.Uwp.Helpers.MessageHelper.ShowDanger("由于 Issues 限制，图片过大，建议使用预设图或 SVG");
                        VM.ImgIcon = null;
                    }
                    else
                    {
                        try
                        {
                            VM.ImgIcon = await ImgHelper.GetImg(icon.Value);
                        }
                        catch
                        {
                            VM.ImgIcon = null;
                        }
                    }
                    break;
                case IconType.Path:
                    try
                    {
                        VM.PathIcon = ImgHelper.PathMarkupToGeometry(icon.Value);
                    }
                    catch
                    {
                        VM.PathIcon = null;
                    }
                    break;
            }
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Border border = sender as Border;
            border.CornerRadius = new CornerRadius(border.ActualWidth * 0.5);
        }
    }
}
