using System.Collections.Generic;

namespace Lavcode.Service.Api.Dtos
{
    public class CreatePasswordDto
    {
        public string FolderId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }

        public UpsertIconDto Icon { get; set; }
        public IList<UpsertKeyValuePairDto> KeyValuePairs { get; set; }
    }
}
