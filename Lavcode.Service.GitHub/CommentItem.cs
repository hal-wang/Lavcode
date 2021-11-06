using Octokit;

namespace Lavcode.Service.GitHub
{
    internal class CommentItem<T>
    {
        public IssueComment Comment { get; set; }
        public T Value { get; set; }
    }
}
