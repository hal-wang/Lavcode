using System.Collections.Generic;

namespace Lavcode.Service.Api.Dtos
{
    public class UpdatePasswordDto
    {
        public string FolderId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public int Order { get; set; }

        public UpsertIconDto Icon { get; set; }
        public IList<UpsertKeyValuePairDto> KeyValuePairs { get; set; }
    }
}
