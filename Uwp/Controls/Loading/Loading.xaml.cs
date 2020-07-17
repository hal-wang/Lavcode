using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.Controls.Loading
{
    internal sealed partial class Loading : PopupLayout, ILoading
    {
        public Loading()
        {
            this.InitializeComponent();
        }

        internal void Show(string loadingStr, bool clickable, double backgroundOpacity, double paneOpacity)
        {
            LoadingStr = loadingStr;
            Clickable = clickable;
            BackgroundOpacity = backgroundOpacity;
            PaneOpacity = paneOpacity;

            if (!IsOpen)
            {
                IsOpen = true;
            }
        }

        public void Hide() => IsOpen = false;

        #region 依赖属性
        public bool Clickable
        {
            get { return (bool)GetValue(ClickableProperty); }
            set { SetValue(ClickableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clickable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickableProperty =
            DependencyProperty.Register("Clickable", typeof(bool), typeof(ILoading), new PropertyMetadata(false));



        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity", typeof(double), typeof(ILoading), new PropertyMetadata(0.0));


        public double PaneOpacity
        {
            get { return (double)GetValue(PaneOpacityProperty); }
            set { SetValue(PaneOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PaneOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PaneOpacityProperty =
            DependencyProperty.Register("PaneOpacity", typeof(double), typeof(ILoading), new PropertyMetadata(0.6));




        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(ILoading), new PropertyMetadata(40.0));




        public string LoadingStr
        {
            get { return (string)GetValue(LoadingStrProperty); }
            set { SetValue(LoadingStrProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadingStr.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingStrProperty =
            DependencyProperty.Register("LoadingStr", typeof(string), typeof(ILoading), new PropertyMetadata(string.Empty));
        #endregion
    }
}
