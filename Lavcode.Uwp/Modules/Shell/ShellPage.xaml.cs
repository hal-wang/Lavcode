using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Modules.SqliteSync;
using Lavcode.Uwp.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class ShellPage : Page
    {
        private bool _inited = false;
        private readonly ThemeListener _themeListener = new();
        public ShellPage()
        {
            DataContext = VM;
            InitializeComponent();
            TitleBarHelper.SetTitleBar();
            _themeListener.ThemeChanged += ThemeListener_ThemeChanged;
            Loaded += MainPage_Loaded;
        }

        public ShellPageViewModel VM { get; } = SimpleIoc.Default.GetInstance<ShellPageViewModel>();

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_inited)
            {
                return;
            }
            _inited = true;

            if (param is not null and StorageFile file)
            {
                SimpleIoc.Default.GetInstance<SqliteFileService>().OpenedFile = file;
                FindName(nameof(SyncFileHandler));
            }
            else
            {
                LoadUI();
            }
        }

        ~ShellPage()
        {
            _themeListener.ThemeChanged -= ThemeListener_ThemeChanged;
        }

        private void ThemeListener_ThemeChanged(ThemeListener sender)
        {
            TitleBarHelper.SetTitleBar();
        }

        private object param = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Frame.BackStack.Clear();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            param = e.Parameter;
        }

        /// <summary>
        /// lazy load
        /// </summary>
        public void LoadUI()
        {
            FindName(nameof(PasswordDetail));
            FindName(nameof(PasswordList));
            FindName(nameof(FolderList));
        }
    }
}
