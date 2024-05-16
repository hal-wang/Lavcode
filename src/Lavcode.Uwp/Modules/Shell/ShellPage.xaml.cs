using HTools.Uwp.Helpers;
using Lavcode.Uwp.Modules.SqliteSync;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class ShellPage : Page
    {
        private readonly ThemeListener _themeListener = new();
        public ShellPage()
        {
            InitializeComponent();
            TitleBarHelper.SetTitleBar();
            Loaded += MainPage_Loaded;
            Unloaded += ShellPage_Unloaded;
            OpenedFile = ServiceProvider.Services.GetService<SqliteFileService>()?.OpenedFile;
            if (OpenedFile != null)
            {
                var sfs = ServiceProvider.Services.GetService<SqliteFileService>();
                sfs.OnOpenedFileChange += () => OpenedFile = sfs.OpenedFile;
            }
        }

        public StorageFile OpenedFile
        {
            get { return (StorageFile)GetValue(OpenedFileProperty); }
            set { SetValue(OpenedFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenedFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenedFileProperty =
            DependencyProperty.Register("OpenedFile", typeof(StorageFile), typeof(ShellPage), new PropertyMetadata(null));


        public bool IsSearchOpen
        {
            get { return (bool)GetValue(IsSearchOpenProperty); }
            set { SetValue(IsSearchOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSearchOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSearchOpenProperty =
            DependencyProperty.Register("IsSearchOpen", typeof(bool), typeof(ShellPage), new PropertyMetadata(false));


        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _themeListener.ThemeChanged += ThemeListener_ThemeChanged;

            if (OpenedFile is not null and StorageFile)
            {
                FindName(nameof(SyncFileHandler));
            }
            else
            {
                LoadUI();
            }
        }

        private void ShellPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _themeListener.ThemeChanged -= ThemeListener_ThemeChanged;
        }

        private void ThemeListener_ThemeChanged(ThemeListener sender)
        {
            TitleBarHelper.SetTitleBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Frame.BackStack.Clear();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// lazy load
        /// </summary>
        public void LoadUI()
        {
            FindName(nameof(PasswordDetail));
            FindName(nameof(PasswordList));
            FindName(nameof(FolderList));
            FindName(nameof(SearchBar));
        }
    }
}
