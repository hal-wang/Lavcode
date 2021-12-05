namespace Lavcode.Service.BaseGit.Models
{
    public class CommentItem<T>
    {
        public int Id { get; set; }
        public T Value { get; set; }
    }
}
