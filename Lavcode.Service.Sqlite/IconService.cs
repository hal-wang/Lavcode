using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class IconService : IIconService
    {
        private readonly SQLiteConnection _con;

        public IconService(IConService cs)
        {
            _con = (cs as ConService).Connection;
        }

        public async Task<Icon> GetIcon(string sourceId)
        {
            Icon result = null;
            await TaskExtend.Run(() =>
            {
                result = _con.Table<Icon>().Where((item) => item.Id == sourceId).FirstOrDefault();
            });
            return result;
        }
    }
}
