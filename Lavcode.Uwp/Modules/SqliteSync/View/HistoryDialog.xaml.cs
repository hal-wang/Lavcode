using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Controls.Message;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Modules.SqliteSync.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.SqliteSync.View
{
    public sealed partial class HistoryDialog : ContentDialog
    {
        public HistoryDialog()
        {
            DataContext = VM;
            this.InitializeComponent();

            Loaded += (s, e) => VM.Init();
        }

        public SyncHistoryViewModel VM { get; } = SimpleIoc.Default.GetInstance<SyncHistoryViewModel>();

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageHelper.ShowSticky(sender as FrameworkElement, "仅记录本地\n不记录 云端 和 导出文件", MessageType.Info);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VM.Delete((sender as FrameworkElement).Tag as string);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            VM.Export((sender as FrameworkElement).Tag as string);
        }
    }
}
