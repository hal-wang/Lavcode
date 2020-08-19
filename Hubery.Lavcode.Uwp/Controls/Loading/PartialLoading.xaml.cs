using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.Controls.Loading
{
    public sealed partial class PartialLoading : UserControl, ILoading
    {
        public PartialLoading()
        {
            this.InitializeComponent();
        }

        #region 依赖属性
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PartialLoading), new PropertyMetadata(false));


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
