using SQLite;

namespace Hubery.Lavcode.Uwp.Model
{
    /// <summary>
    /// 记录已删除的文件夹或密码，用于备份恢复
    /// </summary>
    public class DelectedItem
    {
        /// <summary>
        /// 默认构造函数，给Sqlite用
        /// </summary>
        public DelectedItem() { }

        public DelectedItem(string sourceId, StorageType storageType)
        {
            Id = sourceId;
            StorageType = storageType;
        }

        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 文件夹还是密码
        /// </summary>
        public StorageType StorageType { get; set; }
    }
}