namespace Lavcode.Uwp
{
    /*
     * GitHub相关
     */
    public static partial class Global
    {
        public static string GitAccount { get; } = "hal-wang";
        public static string Repos { get; } = "Lavcode";
        public static int FeedbackIssueNumber { get; } = 1;
        public static int NoticeIssueNumber { get; } = 2;
        public static string GitUrl { get; } = "https://github.com";
        public static string GitBaseApi { get; } = "https://api.github.com";
        public static string ReposUrl { get; } = $"{GitUrl}/{GitAccount}/{Repos}";
        public static string AuthTitleKey { get; } = "lavcode_github_token";
    }
}
