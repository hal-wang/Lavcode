namespace Lavcode.Uwp.SqliteSync.Model
{
    public class CloudItem
    {
        public CloudItem() { }

        public CloudItem(CloudType cloudType, string name, string url)
        {
            CloudType = cloudType;
            Name = name;
            Url = url;
        }

        public CloudType CloudType { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
