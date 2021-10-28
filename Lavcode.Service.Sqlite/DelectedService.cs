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
        private readonly SQLiteConnection _con;

        public DelectedService(IConService cs)
        {
            _con = (cs as ConService).Connection;
        }

        public async Task<List<DelectedItem>> GetDelectedItems()
        {
            List<DelectedItem> result = null;
            await TaskExtend.Run(() =>
            {
                result = _con.Table<DelectedItem>().ToList();
            });
            return result;
        }

        public async Task Add(DelectedItem delectedItem)
        {
            await TaskExtend.Run(() =>
            {
                _con.Insert(delectedItem);
            });
        }

        public async Task Add(IList<DelectedItem> delectedItems)
        {
            await TaskExtend.Run(() =>
            {
                _con.InsertAll(delectedItems);
            });
        }
    }
}
