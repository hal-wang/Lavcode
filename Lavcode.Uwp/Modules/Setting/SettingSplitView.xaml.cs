using GalaSoft.MvvmLight.Ioc;

namespace Lavcode.Uwp.Modules.Setting
{
    public sealed partial class SettingSplitView : HTools.Uwp.Controls.Setting.SettingSplitView
    {
        public SettingSplitView()
        {
            DataContext = VM;
            this.InitializeComponent();
        }

        public SettingViewModel VM { get; } = new SettingViewModel();


        private void OnChangeProvider(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(FirstUse.FirstUsePage));
            IsPaneOpen = false;
        }
    }
}
