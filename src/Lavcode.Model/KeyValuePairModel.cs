namespace Lavcode.Model
{
    public class KeyValuePairModel
    {
        public string Id { get; set; }
        public string PasswordId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValuePairModel DeepClone()
        {
            return new KeyValuePairModel()
            {
                Id = Id,
                Key = Key,
                PasswordId = PasswordId,
                Value = Value,
            };
        }
    }
}
