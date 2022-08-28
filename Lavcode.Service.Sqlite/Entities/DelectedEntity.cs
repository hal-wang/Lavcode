using Lavcode.Model;
using SQLite;

namespace Lavcode.Service.Sqlite.Entities
{
    /// <summary>
    /// 记录已删除的文件夹或密码，用于备份恢复
    /// </summary>
    [Table("DelectedItem")]
    public class DelectedEntity
    {
        /// <summary>
        /// 默认构造函数，给Sqlite用
        /// </summary>
        public DelectedEntity() { }

        public DelectedEntity(string sourceId, StorageType storageType)
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