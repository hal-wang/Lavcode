using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Controls.IconControl
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


        private static async void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            switch (icon.IconType)
            {
                case IconType.SegoeMDL2:
                    iconControl.Model.SegoeMDL2Icon = (icon.Value != null && icon.Value.Length > 0) ? icon.Value[0].ToString() : string.Empty;
                    break;
                case IconType.Img:
                    if ((SettingHelper.Instance.Provider == Provider.Gitee || SettingHelper.Instance.Provider == Provider.GitHub)
                        && icon.Value.Length > 60000)
                    {
                        HTools.Uwp.Helpers.MessageHelper.ShowDanger("由于 Issues 限制，图片过大，建议使用预设图或 SVG");
                        iconControl.Model.ImgIcon = null;
                    }
                    else
                    {
                        try
                        {
                            iconControl.Model.ImgIcon = await ImgHelper.GetImg(icon.Value);
                        }
                        catch
                        {
                            iconControl.Model.ImgIcon = null;
                        }
                    }
                    break;
                case IconType.Path:
                    try
                    {
                        iconControl.Model.PathIcon = ImgHelper.PathMarkupToGeometry(icon.Value);
                    }
                    catch
                    {
                        iconControl.Model.PathIcon = null;
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
