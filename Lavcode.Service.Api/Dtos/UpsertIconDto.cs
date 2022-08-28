using Lavcode.Model;

namespace Lavcode.Service.Api.Dtos
{
    public class UpsertIconDto
    {
        public IconType IconType { get; set; }
        public string Value { get; set; }
    }
}
