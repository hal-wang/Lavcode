using Hubery.Lavcode.Uwp;
using Hubery.Lavcode.Uwp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Helpers.Sqlite
{
    public partial class SqliteHelper
    {
        public async Task DeleteFolder(string folderId, bool record = true)
        {
            if (Table<Folder>().Where((item) => item.Id == folderId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                RunInTransaction(() =>
                {
                    if (record)
                    {
                        foreach (var folder in Table<Folder>().Where((item) => item.Id == folderId).ToList())
                        {
                            Insert(new DelectedItem(folder.Id, StorageType.Folder));
                        }
                    }

                    Table<Icon>().Where((item) => item.Id == folderId).Delete();
                    foreach (var id in Table<Password>().Where((item) => item.FolderId == folderId).Select((item) => item.Id).ToList())
                    {
                        DeletePasswordItem(id, record);
                    }
                    Table<Folder>().Where((item) => item.Id == folderId).Delete();
                });
            });

            DbChanged();
        }

        public async Task AddFolder(Folder folder, Icon icon)
        {
            await TaskExtend.Run(() =>
            {
                folder.LastEditTime = DateTime.Now;

                RunInTransaction(() =>
                {
                    if (folder.Order == 0)
                    {
                        int order = 1;
                        var maxOrder = Table<Folder>().OrderByDescending((item) => item.Order).Select((item) => item.Order).FirstOrDefault();
                        if (maxOrder != default)
                        {
                            order = maxOrder + 1;
                        }
                        folder.Order = order;
                        Insert(folder);
                    }

                    icon.Id = folder.Id;
                    Insert(icon);
                });
            });

            DbChanged();
        }

        public async Task UpdateFolder(Folder folder, Icon icon = null)
        {
            await TaskExtend.Run(() =>
            {
                RunInTransaction(() =>
                {
                    if (folder != null)
                    {
                        folder.LastEditTime = DateTime.Now;
                        Update(folder);
                    }

                    if (icon != null)
                    {
                        Update(icon);
                    }
                });
            });

            DbChanged();
        }

        public async Task<List<Folder>> GetFolders()
        {
            List<Folder> folders = null;
            await TaskExtend.Run(() =>
            {
                folders = Table<Folder>().OrderBy((item) => item.Order).ToList();
            });
            return folders;
        }
    }
}
