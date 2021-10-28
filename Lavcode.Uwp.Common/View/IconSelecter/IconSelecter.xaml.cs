using HTools.Uwp.Helpers;
using Lavcode.Model;
using Microsoft.Toolkit.Uwp;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Common.View.IconSelecter
{
    public sealed partial class IconSelecter : UserControl
    {
        public IconSelecter()
        {
            this.InitializeComponent();
        }

        public IncrementalLoadingCollection<IconSource, string> Icons { get; } = new IncrementalLoadingCollection<IconSource, string>();

        public Icon Icon
        {
            get { return (Icon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Icon), typeof(IconSelecter), new PropertyMetadata(null));

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is string str)
            {
                var icon = Icon.DeepClone();
                icon.IconType = IconType.SegoeMDL2;
                icon.Value = str;
                Icon = icon;

                IconFlyout.Hide();
            }
        }

        public async void HandleSelectImg()
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".bmp");
                picker.FileTypeFilter.Add(".ico");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file == null)
                {
                    return;
                }

                using var fileStream = await file.OpenReadAsync();
                var data = new byte[fileStream.Size].AsBuffer();
                await fileStream.ReadAsync(data, (uint)data.Length, InputStreamOptions.None);

                var icon = Icon.DeepClone();
                icon.IconType = IconType.Img;
                icon.Value = Convert.ToBase64String(data.ToArray());
                Icon = icon;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        public async void HandlePathIcon()
        {
            var setPathIcon = new SetPathIcon();
            if (await setPathIcon.QueueAsync() != ContentDialogResult.Primary)
            {
                return;
            }

            var icon = Icon.DeepClone();
            icon.IconType = IconType.Path;
            icon.Value = setPathIcon.PathStr;
            Icon = icon;
        }
    }
}