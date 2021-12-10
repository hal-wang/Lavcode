using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class PasswordService : IPasswordService
    {
        private readonly ConService _cs;
        private SQLiteConnection Connection => _cs.Connection;

        public PasswordService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task AddPassword(Password password, Icon icon, List<KeyValuePair> keyValuePairs = null)
        {
            await TaskExtend.Run(() =>
            {
                password.LastEditTime = DateTime.Now;

                if (password.Order == 0)
                {
                    var order = Connection
                        .Table<Password>()
                        .Where((item) => item.FolderId == password.FolderId)
                        .OrderByDescending((item) => item.Order)
                        .Take(1)
                        .Select((item) => item.Order)
                        .FirstOrDefault();

                    password.Order = order + 1;
                }

                // 如果有重复
                if (Connection.Table<Password>().Where((item) => item.Id == password.Id).Count() > 0)
                {
                    password.SetNewId();
                }

                Connection.RunInTransaction(() =>
                {
                    Connection.Insert(password);
                    icon.Id = password.Id;
                    Connection.Insert(icon);

                    if (keyValuePairs != null)
                    {
                        foreach (var kvp in keyValuePairs)
                        {
                            kvp.Id = default;
                            kvp.SourceId = password.Id;
                        }
                        Connection.InsertAll(keyValuePairs);
                    }
                });
            });
        }

        public async Task DeletePassword(string passwordId, bool record = true)
        {
            if (Connection.Table<Password>().Where((item) => item.Id == passwordId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                Connection.RunInTransaction(() =>
                {
                    Connection.Table<Password>().Where((item) => item.Id == passwordId).Delete();
                    Connection.Table<Icon>().Where((item) => item.Id == passwordId).Delete();
                    Connection.Table<KeyValuePair>().Where(item => item.SourceId == passwordId).Delete();

                    if (record)
                    {
                        Connection.Insert(new DelectedItem(passwordId, StorageType.Password));
                    }
                });
            });
        }

        public async Task<List<KeyValuePair>> GetKeyValuePairs(string passwordId)
        {
            List<KeyValuePair> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<KeyValuePair>().Where((item) => item.SourceId == passwordId).ToList();
            });
            return result;
        }

        public async Task<List<Password>> GetPasswords(string folderId)
        {
            List<Password> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<Password>().Where((item) => item.FolderId == folderId).OrderBy((item) => item.Order).ToList();
            });
            return result;
        }

        public async Task<List<Password>> GetPasswords()
        {
            List<Password> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<Password>().OrderBy((item) => item.FolderId).ThenBy((item) => item.Order).ToList();
            });
            return result;
        }

        public async Task UpdatePassword(Password password, Icon icon = null, List<KeyValuePair> keyValuePairs = null)
        {
            await TaskExtend.Run(() =>
            {
                password.LastEditTime = DateTime.Now;

                Connection.RunInTransaction(() =>
                {
                    Connection.Update(password);
                    if (icon != null)
                    {
                        Connection.Update(icon);
                    }

                    if (keyValuePairs != null)
                    {
                        Connection.Table<KeyValuePair>().Where((item) => item.SourceId == password.Id).Delete();
                        foreach (var kvp in keyValuePairs)
                        {
                            kvp.Id = default;
                            kvp.SourceId = password.Id;
                        }
                        Connection.InsertAll(keyValuePairs);
                    }
                });
            });
        }
    }
}
