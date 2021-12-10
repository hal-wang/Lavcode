using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class IconService : IIconService
    {
        private readonly ConService _cs;
        private SQLiteConnection Connection => _cs.Connection;

        public IconService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task<Icon> GetIcon(string sourceId)
        {
            Icon result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<Icon>().Where((item) => item.Id == sourceId).FirstOrDefault();
            });
            return result;
        }
    }
}
