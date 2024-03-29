﻿using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Auth;
using Lavcode.Uwp.Modules.FirstUse;
using Lavcode.Uwp.Modules.Shell;
using Lavcode.Uwp.Modules.SqliteSync;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Linq;
using System.Web;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp
{
    sealed partial class App : Application
    {
        public static Frame Frame { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
            ServiceProvider.RegisterSimple();
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageHelper.ShowError(e.Exception, 0, "UnhandledException" + sender?.GetType()?.ToString());
                e.Handled = true;
            }
            catch { }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);

#if DEBUG
#else
            UpdateHelper.DownloadAndInstallAllUpdatesBackground();
#endif

            CreateFrame();
            if (!e.PrelaunchActivated)
            {
                SimpleNavFirstPage();
            }
            Window.Current.Activate();
        }

        protected override void OnFileActivated(FileActivatedEventArgs e)
        {
            CreateFrame();

            if (!SettingHelper.Instance.IsFirstInited || e.Files.FirstOrDefault() is not StorageFile file)
            {
                SimpleNavFirstPage();
            }
            else if (Frame.Content == null)
            {
                ServiceProvider.Register<Service.Sqlite.ConService>();
                ServiceProvider.Services.GetService<SqliteFileService>().OpenedFile = file;
                Frame.Navigate(typeof(ShellPage), file);
            }
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            var isOpened = Window.Current.Content is Frame;
            CreateFrame();
            if (!isOpened)
            {
                if (args is CommandLineActivatedEventArgs commandArgs)
                {
                    var commandProvider = new CommandLauncher(commandArgs.Operation.Arguments).Provider;
                    if (commandProvider != null)
                    {
                        SettingHelper.Instance.Provider = commandProvider.Value;
                    }
                }
                SimpleNavFirstPage();
            }
            else if (args.Kind == ActivationKind.Protocol)
            {
                var eventArgs = args as ProtocolActivatedEventArgs;
                var query = HttpUtility.ParseQueryString(eventArgs.Uri.Query);
                if (!query.AllKeys.Contains("provider")) return;
                switch (query["provider"])
                {
                    case "github":
                        StrongReferenceMessenger.Default.Send(query, $"On{Provider.GitHub}Notify");
                        return;
                    case "gitee":
                        StrongReferenceMessenger.Default.Send(query, $"On{Provider.Gitee}Notify");
                        return;
                }
            }
            Window.Current.Activate();
        }

        private void SimpleNavFirstPage()
        {
            if (Frame.Content == null)
            {
                if (SettingHelper.Instance.IsFirstInited)
                {
                    Frame.Navigate(typeof(AuthPage));
                }
                else
                {
                    Frame.Navigate(typeof(FirstUsePage));
                }
            }
            Window.Current.Activate();
        }

        private void CreateFrame()
        {
            if (Window.Current.Content is Windows.UI.Xaml.Controls.Frame)
            {
                return;
            }

            Frame = new Frame();
            Frame.NavigationFailed += OnNavigationFailed;
            Frame.Navigated += OnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            Window.Current.Content = Frame;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            MessageHelper.ShowDanger("跳转失败" + e.SourcePageType.FullName);
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (LoadingHelper.IsLoading ||
                Frame == null ||
                e.Handled ||
                !Frame.CanGoBack ||
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Collapsed)
            {
                return;
            }

            e.Handled = true;
            Frame.GoBack();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}