using Lavcode.Uwp.Model;
using HTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Helpers.Sqlite
{
    public partial class SqliteHelper
    {
        public async Task DeletePassword(string passwordId, bool record = true)
        {
            if (Table<Password>().Where((item) => item.Id == passwordId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                RunInTransaction(() =>
                {
                    DeletePasswordItem(passwordId, record);
                });
            });

            DbChanged();
        }

        private void DeletePasswordItem(string passwordId, bool record = true)
        {
            if (Table<Password>().Where((item) => item.Id == passwordId).Count() == 0)
            {
                return;
            }

            if (record)
            {
                foreach (var password in Table<Password>().Where((item) => item.Id == passwordId).ToList())
                {
                    Insert(new DelectedItem(password.Id, StorageType.Password));
                }
            }

            Table<Model.KeyValuePair>().Where((item) => item.SourceId == passwordId).Delete();
            Table<Icon>().Where((item) => item.Id == passwordId).Delete();
            Table<Password>().Where((item) => item.Id == passwordId).Delete();
        }

        public async Task AddPassword(Password password, Icon icon, List<Model.KeyValuePair> keyValuePairs = null)
        {
            await TaskExtend.Run(() =>
            {
                password.LastEditTime = DateTime.Now;

                if (password.Order == 0)
                {
                    int order = 0;
                    var orderQuery = Table<Password>().Where((item) => item.FolderId == password.FolderId).OrderByDescending((item) => item.Order).Select((item) => item.Order).Take(1).ToList();
                    if (orderQuery.Count() > 0)
                    {
                        order = orderQuery.First() + 1;
                    }

                    password.Order = order;
                }

                // 如果有重复
                if (Table<Password>().Where((item) => item.Id == password.Id).Count() > 0)
                {
                    password.SetNewId();
                }

                RunInTransaction(() =>
                {
                    Insert(password);
                    icon.Id = password.Id;
                    Insert(icon);

                    if (keyValuePairs != null)
                    {
                        foreach (var kvp in keyValuePairs)
                        {
                            kvp.Id = default;
                            kvp.SourceId = password.Id;
                            Insert(kvp);
                        }
                    }
                });
            });

            DbChanged();
        }

        public async Task UpdatePassword(Password password, Icon icon = null, List<Model.KeyValuePair> keyValuePairs = null)
        {
            await TaskExtend.Run(() =>
            {
                password.LastEditTime = DateTime.Now;

                RunInTransaction(() =>
                {
                    Update(password);
                    if (icon != null)
                    {
                        Update(icon);
                    }

                    if (keyValuePairs != null)
                    {
                        Table<Model.KeyValuePair>().Where((item) => item.SourceId == password.Id).Delete();
                        foreach (var kvp in keyValuePairs)
                        {
                            kvp.Id = default;
                            kvp.SourceId = password.Id;
                            Insert(kvp);
                        }
                    }
                });
            });

            DbChanged();
        }

        public async Task<List<Password>> GetPasswords(string folderId)
        {
            List<Password> result = null;
            await TaskExtend.Run(() =>
            {
                result = Table<Password>().Where((item) => item.FolderId == folderId).OrderBy((item) => item.Order).ToList();
            });
            return result;
        }

        public async Task<List<Password>> GetPasswords()
        {
            List<Password> result = null;
            await TaskExtend.Run(() =>
            {
                result = Table<Password>().ToList();
            });
            return result;
        }
    }
}
