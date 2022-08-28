namespace Lavcode.Service.Api.Dtos
{
    public class UpdateFolderDto
    {
        public string Name { get; set; }
        public int Order { get; set; }

        public UpsertIconDto Icon { get; set; }
    }
}
