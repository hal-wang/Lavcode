namespace Hubery.Lavcode.Uwp
{
    /*
     * GitHub相关
     */
    public static partial class Global
    {
        public static string GitHubAccount { get; } = "hbrwang";
        public static string Repos { get; } = "Lavcode";
        public static string GitHubToken { get; } = "17d0f9b3d9407b64b8ef470980b38c013a789e36 ";
        public static int FeedbackIssueNumber { get; } = 1;
        public static int NoticeIssueNumber { get; } = 2;
        public static string GitHubUrl { get; } = "https://github.com";
        public static string ReposUrl { get; } = $"{GitHubUrl}/{GitHubAccount}/{Repos}";
    }
}
