namespace Hubery.Lavcode.Uwp
{
    /*
     * Git相关
     */
    public static partial class Global
    {
        public static string GiteeAccount { get; } = "hbrwang";
        public static string Repos { get; } = "Lavcode";
        public static string FeedbackIssueId { get; } = "I1PO2Z";
        public static string NoticeIssueId { get; } = "I1PO2Y";
        public static string GiteeUrl { get; } = "https://gitee.com";
        public static string ReposUrl { get; } = $"{GiteeUrl}/{GiteeAccount}/{Repos}";
        public static string GiteeBaseApiUrl { get; } = $"{GiteeUrl}/api/v5";
        public static string GiteeReposApiUrl { get; } = $"{GiteeAccount}/{Repos}";
        public static string GiteeClientId { get; } = "ab5b9883b6444750ff708e47d98a04f57ffc033306ec89271325356e5b5d8312";
        public static string GiteeClientSecret { get; } = "94c9229f3ef49b509618dd39f7c0792e6fcc569c1c45e6651adfa6e4bf5d2a97";
    }
}
