using Hubery.Tools.Uwp.Controls.Dialog;
using System;
using Windows.System;

namespace Lavcode.Uwp.View.Feedback
{
    public sealed partial class MarkdownPreview : LayoutDialog
    {
        public MarkdownPreview(string markdown)
        {
            this.InitializeComponent();

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
