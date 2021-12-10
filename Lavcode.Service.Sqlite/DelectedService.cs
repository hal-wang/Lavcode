using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class DelectedService : IDelectedService
    {
        private readonly ConService _cs;
        private SQLiteConnection Connection => _cs.Connection;

        public DelectedService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task<List<DelectedItem>> GetDelectedItems()
        {
            List<DelectedItem> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<DelectedItem>().ToList();
            });
            return result;
        }

        public async Task Add(DelectedItem delectedItem)
        {
            await TaskExtend.Run(() =>
            {
                Connection.Insert(delectedItem);
            });
        }

        public async Task Add(IList<DelectedItem> delectedItems)
        {
            await TaskExtend.Run(() =>
            {
                Connection.InsertAll(delectedItems);
            });
        }
    }
}
