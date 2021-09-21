using HTools.Uwp.Helpers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.PasswordList
{
    public sealed partial class MoveToDialog : ContentDialog, IResultDialog<bool>
    {
        public MoveToDialog(Model.Folder curFolder, IReadOnlyList<Model.Password> passwords)
        {
            this.InitializeComponent();

            Model.Init(curFolder, passwords);
        }

        public bool Result { get; private set; } = false;

        private async void LayoutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;

            try
            {
                if (await Model.MoveTo())
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
