using HTools.Uwp.Controls;
using HTools.Uwp.Helpers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.PasswordList
{
    public sealed partial class MoveToDialog : LayoutDialog
    {
        public MoveToDialog(Model.Folder curFolder, IReadOnlyList<Model.Password> passwords)
        {
            this.InitializeComponent();

            Model.Init(curFolder, passwords);
        }

        private async void LayoutDialog_PrimaryButtonClick(LayoutDialog sender, LayoutDialogButtonClickEventArgs args)
        {
            args.Cancel = true;

            try
            {
                if (await Model.MoveTo())
                {
                    this.Result = ContentDialogResult.Primary;
                    this.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
