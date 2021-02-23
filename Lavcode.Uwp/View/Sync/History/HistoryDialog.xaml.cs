using HTools.Uwp.Controls;
using HTools.Uwp.Controls.Message;
using HTools.Uwp.Helpers;
using Windows.UI.Xaml;

namespace Lavcode.Uwp.View.Sync.History
{
    public sealed partial class HistoryDialog : LayoutDialog
    {
        public HistoryDialog()
        {
            this.InitializeComponent();

            Loaded += HistoryDialog_Loaded;
        }

        private void HistoryDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Model.Init();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageHelper.ShowSticky(sender as FrameworkElement, "仅记录本地\n不记录 云端 和 导出文件", MessageType.Info);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Model.Delete((sender as FrameworkElement).Tag as string);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Model.Export((sender as FrameworkElement).Tag as string);
        }
    }
}
