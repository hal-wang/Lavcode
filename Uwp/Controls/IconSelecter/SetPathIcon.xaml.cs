using Hubery.Lavcode.Uwp.Controls.Dialog;
using Hubery.Lavcode.Uwp.Helpers;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Hubery.Lavcode.Uwp.Controls.IconSelecter
{
    public sealed partial class SetPathIcon : LayoutDialog
    {
        public SetPathIcon()
        {
            this.InitializeComponent();

            Loaded += SetPathIcon_Loaded;
        }

        private async void SetPathIcon_Loaded(object sender, RoutedEventArgs e)
        {
            if (SettingHelper.Instance.SvgTaught)
            {
                return;
            }

            await PopupHelper.ShowTeachingTip(ResultBorder, "路径图结果（路径图 1/2）", "这里显示路径图的结果");
            await PopupHelper.ShowTeachingTip(TextBoxElement, "路径图内容（路径图 2/2）", "在这里输入路径图内容，推荐在 www.iconfont.cn 网站找到图标，复制SVG文本内容，粘贴到此处。");
            SettingHelper.Instance.SvgTaught = true;
        }

        public string PathStr
        {
            get { return (string)GetValue(PathStrProperty); }
            set { SetValue(PathStrProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathStr.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathStrProperty =
            DependencyProperty.Register("PathStr", typeof(string), typeof(SetPathIcon), new PropertyMetadata(string.Empty, OnPathStrChanged));

        private static void OnPathStrChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetPathIcon setPathIcon = d as SetPathIcon;
            try
            {
                setPathIcon.Data = ImgHelper.PathMarkupToGeometry(setPathIcon.PathStr);
                //{
                //    Data = (Geometry)TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom(e.NewValue as string)
                //};
            }
            catch
            {
                setPathIcon.Data = null;
            }
        }

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(SetPathIcon), new PropertyMetadata(null));

        private void LayoutDialog_PrimaryButtonClick(LayoutDialog sender, LayoutDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(PathStr))
            {
                MessageHelper.ShowWarning("请输入路径");
                args.Cancel = true;
                return;
            }

            if (Data == null)
            {
                MessageHelper.ShowWarning("路径无效");
                args.Cancel = true;
                return;
            }
        }

        private async void Help_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uriBing = new Uri("https://blog.hubery.wang/aa87f3dc");
                var success = await Launcher.LaunchUriAsync(uriBing);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}