using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace Lavcode.Helpers
{
    public static class ImgHelper
    {
        public static async Task<BitmapImage> GetImg(string base64)
        {
            return await GetImg(System.Convert.FromBase64String(base64).AsBuffer());
        }

        public static async Task<BitmapImage> GetImg(IBuffer data)
        {
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(data);
            stream.Seek(0);

            BitmapImage bitmapImage = null;
            if (Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.HasThreadAccess)
            {
                bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
            }
            else
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                });
            }

            return bitmapImage;
        }

        public static Geometry PathMarkupToGeometry(string pathMarkup)
        {
            var path = PathMarkupToPath(pathMarkup);
            Geometry geometry = path.Data;
            path.Data = null;
            return geometry;
        }

        public static Path PathMarkupToPath(string pathMarkup)
        {
            string path;
            var matches = Regex.Matches(pathMarkup, "<path d=\"([ \\S]{1,}?)\"");
            if (matches.Count == 0)
            {
                path = pathMarkup;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (Match match in matches)
                {
                    sb.Append(match.Groups[1].Value);
                    sb.Append(" ");
                }
                path = sb.ToString();
            }



            string xaml =
            " <Path " +
            "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
            "<Path.Data>" + path + "</Path.Data></Path>";
            return XamlReader.Load(xaml) as Path;
        }
    }
}