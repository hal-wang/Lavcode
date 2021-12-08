namespace Lavcode.Common
{
    public static class CommonConstant
    {
        public static string HomeUrl { get; } = "https://lavcode.hal.wang";
        public static string ToolsApiUrl { get; } = "https://tool.hal.wang/v3";
        public static string Email { get; } = "support@hal.wang";
        public static string PpUrl { get; } = $"{HomeUrl}/pp/zh/";
        public static string DragPasswordHeader { get; } = "Lavcode_P";

        public static string SqliteFileExtension { get; } = ".lcs";
    }
}
