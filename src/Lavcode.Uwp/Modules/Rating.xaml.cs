﻿using HTools.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules
{
    public sealed partial class Rating : UserControl
    {
        public Rating()
        {
            this.InitializeComponent();
        }

        private async void Rating_Click(object sender, RoutedEventArgs e)
        {
            await PopupHelper.ShowRating();
        }
    }
}
