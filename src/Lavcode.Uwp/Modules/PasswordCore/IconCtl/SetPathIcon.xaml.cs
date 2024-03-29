﻿using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Guide;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Lavcode.Uwp.Modules.PasswordCore.IconCtl
{
    public sealed partial class SetPathIcon : ContentDialog
    {
        public SetPathIcon()
        {
            this.InitializeComponent();

            Loaded += SetPathIcon_Loaded;
        }

        private async void SetPathIcon_Loaded(object sender, RoutedEventArgs e)
        {
            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.SvgTaught),
                Total = 2,
                Type = "路径图",
            }
            .Add(new GuideItem()
            {
                Title = "路径图结果",
                Content = "这里显示路径图的结果",
                Target = ResultBorder,
            })
            .Add(new GuideItem()
            {
                Title = "路径图内容",
                Content = "在这里输入路径图内容，推荐在 www.iconfont.cn 网站找到图标，复制SVG文本内容，粘贴到此处。",
                Target = TextBoxElement,
            })
            .End()
            .RunAsync();
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
                setPathIcon.Data = Helpers.ImgHelper.PathMarkupToGeometry(setPathIcon.PathStr);
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

        private void LayoutDialog_PrimaryButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, Windows.UI.Xaml.Controls.ContentDialogButtonClickEventArgs args)
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
                var uriBing = new Uri("https://lavcode.hal.wang/usage/svgicon");
                var success = await Launcher.LaunchUriAsync(uriBing);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}