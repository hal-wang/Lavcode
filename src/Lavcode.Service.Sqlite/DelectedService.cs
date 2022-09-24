using HTools;
using Lavcode.IService;
using Lavcode.Service.Sqlite.Entities;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class DelectedService
    {
        private readonly ConService _cs;
        private SQLiteConnection Connection => _cs.Connection;

        public DelectedService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task<List<DelectedEntity>> GetDelectedItems()
        {
            List<DelectedEntity> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<DelectedEntity>().ToList();
            });
            return result;
        }

        public async Task Add(DelectedEntity delectedItem)
        {
            await TaskExtend.Run(() =>
            {
                Connection.Insert(delectedItem);
            });
        }

        public async Task Add(IList<DelectedEntity> delectedItems)
        {
            await TaskExtend.Run(() =>
            {
                Connection.InsertAll(delectedItems);
            });
        }
    }
}
