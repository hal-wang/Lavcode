using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public class ResourcesHelper
    {
        private static ResourceLoader ResourceLoader { get; set; } = ResourceLoader.GetForCurrentView();

        public static T GetResource<T>(string resource)
        {
            return (T)Application.Current.Resources[resource];
        }

        public static string GetResStr(string resource)
        {
            return ResourceLoader.GetString(resource);
        }
    }
}
