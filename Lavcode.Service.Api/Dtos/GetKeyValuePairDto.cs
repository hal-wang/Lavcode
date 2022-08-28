using Lavcode.Model;

namespace Lavcode.Service.Api.Dtos
{
    public class GetKeyValuePairDto
    {
        public string Id { get; set; }
        public string SourceId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValuePairModel ToModel()
        {
            return new KeyValuePairModel()
            {
                Id = Id,
                SourceId = SourceId,
                Key = Key,
                Value = Value
            };
        }
    }
}
