using System;
using Windows.ApplicationModel;

namespace Lavcode.Uwp
{
    public static partial class Global
    {
        public static string HomeUrl { get; } = "https://lavcode.hubery.wang";
        public static string ToolsApiUrl { get; } = "https://cb-api.hubery.wang/tools";

        public static string Email { get; } = "support@hubery.wang";

        public static string Version { get; } = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

        public static string DragPasswordHeader { get; } = "Lavcode_P";
        public static string PpUrl { get; } = $"{HomeUrl}/pp/zh/";

        public static DateTime PublishTime { get; } = DateTime.Parse("2020-05-09");
    }
}
