using SQLite;
using System;

namespace Hubery.Lavcode.Uwp.Model
{
    public abstract class UniqueItem
    {
        public UniqueItem()
        {
            SetNewId();
        }

        [PrimaryKey]
        public string Id { get; set; }

        public void SetNewId()
        {
            Id = DateTimeOffset.Now.ToUnixTimeMilliseconds() + Guid.NewGuid().ToString("N");
        }
    }
}
