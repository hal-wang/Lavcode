using System;
using Windows.ApplicationModel;

namespace Lavcode
{
    public static partial class Global
    {
        public static string HomeUrl { get; } = "https://lavcode.hal.wang";
        public static string ToolsApiUrl { get; } = "https://tool.hal.wang/v2";

        public static string Email { get; } = "support@hal.wang";

        public static string Version { get; } = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

        public static string DragPasswordHeader { get; } = "Lavcode_P";
        public static string PpUrl { get; } = $"{HomeUrl}/pp/zh/";

        public static DateTime PublishTime { get; } = DateTime.Parse("2020-05-09");
    }
}
