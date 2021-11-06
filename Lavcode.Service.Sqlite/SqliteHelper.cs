using Lavcode.IService;
using System;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class SqliteHelper : IDisposable
    {
        public IConService ConService { get; private set; }
        public IFolderService FolderService { get; private set; }
        public IPasswordService PasswordService { get; private set; }
        public IIconService IconService { get; private set; }
        public IDelectedService DeletedService { get; private set; }
        public IConfigService ConfigService { get; private set; }

        private SqliteHelper() { }
        public async static Task<SqliteHelper> OpenAsync(string filePath)
        {
            var result = new SqliteHelper
            {
                ConService = new ConService(),
            };
            await result.ConService.Connect(new { FilePath = filePath });

            result.FolderService = new FolderService(result.ConService);
            result.PasswordService = new PasswordService(result.ConService);
            result.IconService = new IconService(result.ConService);
            result.DeletedService = new DelectedService(result.ConService);
            result.ConfigService = new ConfigService(result.ConService);

            return result;
        }

        public void Dispose()
        {
            ConService?.Dispose();
        }
    }
}
