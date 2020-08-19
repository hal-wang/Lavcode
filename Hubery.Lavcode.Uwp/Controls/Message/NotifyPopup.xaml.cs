using System;
using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.Controls.Message
{
    internal sealed partial class NotifyPopup : PopupLayout
    {
        private readonly int _duration;
        public NotifyPopup(string text, int duration = 2400)
        {
            this.InitializeComponent();

            Text = text;
            _duration = duration;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NotifyPopup), new PropertyMetadata(""));

        public void Show()
        {
            IsOpen = true;

            SbOut.BeginTime = new TimeSpan(0, 0, 0, 0, _duration);
            SbOut.Begin();
            SbOut.Completed += (ss, ee) => IsOpen = false;
        }
    }
}