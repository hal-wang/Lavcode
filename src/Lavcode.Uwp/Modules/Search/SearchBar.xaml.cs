using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Search
{
    public sealed partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            this.InitializeComponent();
        }

        public bool IsSearchOpen
        {
            get { return (bool)GetValue(IsSearchOpenProperty); }
            set { SetValue(IsSearchOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSearchOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSearchOpenProperty =
            DependencyProperty.Register("IsSearchOpen", typeof(bool), typeof(SearchBar), new PropertyMetadata(false, OnIsSearchOpenChanged));

        private static async void OnIsSearchOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                await Task.Delay(200);
                (d as SearchBar)!.SearchTextBox.Focus(FocusState.Pointer);
            }
        }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));


        private void TextBox_PreviewKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Enter:
                    Search();
                    break;
            }
        }

        private void Exit()
        {
            IsSearchOpen = false;
            StrongReferenceMessenger.Default.Send("", "Search");
        }

        private void Search()
        {
            StrongReferenceMessenger.Default.Send(SearchText, "Search");
        }
    }
}
