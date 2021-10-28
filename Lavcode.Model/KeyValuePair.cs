namespace Lavcode.Model
{
    public class KeyValuePair
    {
        public virtual int Id { get; set; }

        public virtual string SourceId { get; set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }
    }
}