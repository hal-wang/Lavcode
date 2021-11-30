using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.Modules.Feedback
{
    public sealed partial class Icon : ContentControl
    {
        private bool _isEntered = false;

        public Icon()
        {
            this.InitializeComponent();
        }

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(Icon), new PropertyMetadata(null));


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(Icon), new PropertyMetadata(false));



        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _isEntered = true;
            if (!IsOpen)
            {
                IsOpen = true;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isEntered = false;
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            timer.Tick += (ss, ee) =>
            {
                timer.Stop();

                if (!_isEntered)
                {
                    IsOpen = false;
                }
            };
            timer.Start();
        }

        private void OnPointerEntered1(object sender, PointerRoutedEventArgs e)
        {
            _isEntered = true;
            if (!IsOpen)
            {
                IsOpen = true;
            }
        }
    }
}
