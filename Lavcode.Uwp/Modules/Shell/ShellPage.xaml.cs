using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Modules.SqliteSync;
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
        private bool _inited = false;
        private readonly ThemeListener _themeListener = new();
        public ShellPage()
        {
            InitializeComponent();
            TitleBarHelper.SetTitleBar();
            _themeListener.ThemeChanged += ThemeListener_ThemeChanged;
            Loaded += MainPage_Loaded;
            OpenedFile = SimpleIoc.Default.ContainsCreated<SqliteFileService>() ? SimpleIoc.Default.GetInstance<SqliteFileService>()?.OpenedFile : null;
            if (SimpleIoc.Default.ContainsCreated<SqliteFileService>())
            {
                var sfs = SimpleIoc.Default.GetInstance<SqliteFileService>();
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

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_inited)
            {
                return;
            }
            _inited = true;

            if (OpenedFile is not null and StorageFile)
            {
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
        }
    }
}
