using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Lavcode.Uwp.Modules.Guide
{
    public class GuideItem
    {
        public FrameworkElement Target { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public TeachingTipPlacementMode Placement { get; set; } = TeachingTipPlacementMode.Auto;
    }
}
