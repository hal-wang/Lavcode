namespace Lavcode.Common
{
    public static class RepositoryConstant
    {
        public static string GitAccount { get; } = "hal-wang";
        public static string Repos { get; } = "Lavcode";
        public static int FeedbackIssueNumber { get; } = 1;
        public static int NoticeIssueNumber { get; } = 2;
        public static string GitHubUrl { get; } = "https://github.com";
        public static string GitHubRepositoryUrl { get; } = $"{GitHubUrl}/{GitAccount}/{Repos}";
        public static string GiteeUrl { get; } = "https://gitee.com";
        public static string GiteeRepositoryUrl { get; } = $"{GiteeUrl}/{GitAccount}/{Repos}";

        public static string GitStorageRepos { get; } = "LavcodeStorage";
    }
}
