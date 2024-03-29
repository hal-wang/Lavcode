﻿using HTools.Uwp.Helpers;
using Lavcode.Common;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Feedback
{
    public sealed partial class FeedbackDialog : ContentDialog, IResultDialog<Issue>
    {
        public FeedbackDialog()
        {
            DataContext = VM;
            InitializeComponent();
        }

        public FeedbackDialogViewModel VM { get; } = ServiceProvider.Services.GetService<FeedbackDialogViewModel>();

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(FeedbackDialog), new PropertyMetadata(false));

        private async void LayoutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            if (IsLoading) return;

            IsLoading = true;
            if (await VM.Feedback())
            {
                this.Hide(VM.IssueResult);
            }
            IsLoading = false;
        }

        private async void Preview_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new MarkdownPreview(VM.Content).QueueAsync();
        }

        private async void Signup_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(RepositoryConstant.GitHubUrl, UriKind.Absolute));
        }

        public Issue Result { get; set; } = null;
    }
}
