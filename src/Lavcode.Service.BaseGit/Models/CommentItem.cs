namespace Lavcode.Service.BaseGit.Models
{
    public class CommentItem<T>
    {
        public long Id { get; set; }
        public T Value { get; set; }
    }
}
