using HTools.Uwp.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordMoveToDialog : ContentDialog, IResultDialog<bool>
    {
        public PasswordMoveToDialog(Lavcode.Model.Folder curFolder, IReadOnlyList<Lavcode.Model.Password> passwords)
        {
            DataContext = VM;
            this.InitializeComponent();

            VM.Init(curFolder, passwords);
        }

        public PasswordMoveToViewModel VM { get; } = ServiceProvider.Services.GetService<PasswordMoveToViewModel>();

        public bool Result { get; set; } = false;

        private async void LayoutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;

            try
            {
                if (await VM.MoveTo())
                {
                    this.Hide(true);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
