using HTools;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.Sqlite.Entities;
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

        public async Task<IconModel> GetIcon(string sourceId)
        {
            IconEntity result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<IconEntity>().Where((item) => item.Id == sourceId).FirstOrDefault();
            });
            return result.ToModel();
        }
    }
}
