using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.Feedback
{
    public sealed partial class MarkdownPreview : ContentDialog
    {
        public MarkdownPreview(string markdown)
        {
            InitializeComponent();

            Markdown = markdown;
        }

        public string Markdown { get; }

        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }
    }
}
