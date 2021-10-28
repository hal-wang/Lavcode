using System;

namespace Lavcode.IService
{
    public interface IConfigService : IDataService
    {
        public DateTime LastEditTime { get; set; }
    }
}
