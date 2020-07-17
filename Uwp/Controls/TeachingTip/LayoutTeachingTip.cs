using Hubery.Lavcode.Uwp.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.Controls.TeachingTip
{
    internal class LayoutTeachingTip : PopupLayout
    {
        #region 依赖属性
        public FrameworkElement Target
        {
            get { return (FrameworkElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(LayoutTeachingTip), new PropertyMetadata(null));


        public TeachingTipPlacementMode PreferredPlacement
        {
            get { return (TeachingTipPlacementMode)GetValue(PreferredPlacementProperty); }
            set { SetValue(PreferredPlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreferredPlacement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreferredPlacementProperty =
            DependencyProperty.Register("PreferredPlacement", typeof(TeachingTipPlacementMode), typeof(LayoutTeachingTip), new PropertyMetadata(TeachingTipPlacementMode.Auto));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LayoutTeachingTip), new PropertyMetadata(string.Empty));


        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity", typeof(double), typeof(LayoutTeachingTip), new PropertyMetadata(0.4));


        public bool Clickable
        {
            get { return (bool)GetValue(ClickableProperty); }
            set { SetValue(ClickableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clickable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickableProperty =
            DependencyProperty.Register("Clickable", typeof(bool), typeof(LayoutTeachingTip), new PropertyMetadata(false));
        #endregion

        public LayoutTeachingTip(string title = "", object content = null) :
            this()
        {
            Title = title;
            if (content is string str)
            {
                Content = new TextBlock()
                {
                    Text = str,
                    TextWrapping = TextWrapping.Wrap
                };
            }
            else
            {
                Content = content;
            }
            Loaded += LayoutTeachingTip_Loaded;
        }

        private void LayoutTeachingTip_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic);
        }

        public LayoutTeachingTip()
        {
            //this.DefaultStyleResourceUri = new Uri("./Style.xaml", UriKind.Relative);
            //this.DefaultStyleKey = typeof(LayoutTeachingTip);
            if (Style == null)
            {
                Style = ResourcesHelper.GetResource<Style>("LayoutTeachingTip");
            }
        }

        protected Microsoft.UI.Xaml.Controls.TeachingTip TeachingTip = null;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TeachingTip = (Microsoft.UI.Xaml.Controls.TeachingTip)GetTemplateChild("TeachingTip");
            if (TeachingTip == null)
            {
                _openErr = true;
                throw new ArgumentException("模板设置不正确（找不到对应TeachingTip）");
            }

            TeachingTip.Closed += (ss, ee) =>
            {
                _args = ee;
                IsOpen = false;
            };
        }

        private TeachingTipClosedEventArgs _args = null;
        public async Task<TeachingTipClosedEventArgs> ShowAt(FrameworkElement target = null)
        {
            _args = null;

            if (target != null)
            {
                Target = target;
            }

            IsOpen = true;
            Open();

            while (IsOpen)
            {
                await TaskExtend.SleepAsync();
            }

            Closed?.Invoke(this, _args);
            return _args;
        }

        private bool _openErr = false;
        private async void Open()
        {
            if (_openErr || (TeachingTip != null && TeachingTip.IsOpen))
            {
                return;
            }

            if (TeachingTip == null)
            {
                await TaskExtend.SleepAsync();
                Open();
            }
            else
            {
                TeachingTip.IsOpen = true;
            }
        }

        public event TypedEventHandler<LayoutTeachingTip, TeachingTipClosedEventArgs> Closed;
    }
}
