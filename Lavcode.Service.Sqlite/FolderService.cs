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
    public class FolderService : IFolderService
    {
        private readonly SQLiteConnection _con;

        public FolderService(IConService cs)
        {
            _con = (cs as ConService).Connection;
        }

        public async Task AddFolder(Folder folder, Icon icon)
        {
            await TaskExtend.Run(() =>
            {
                folder.LastEditTime = DateTime.Now;

                _con.RunInTransaction(() =>
                {
                    if (folder.Order == 0)
                    {
                        int order = 1;
                        var maxOrder = _con.Table<Folder>().OrderByDescending((item) => item.Order).Select((item) => item.Order).FirstOrDefault();
                        if (maxOrder != default)
                        {
                            order = maxOrder + 1;
                        }
                        folder.Order = order;
                    }

                    _con.Insert(folder);
                    icon.Id = folder.Id;
                    _con.Insert(icon);
                });
            });
        }

        public async Task DeleteFolder(string folderId, bool record = true)
        {
            if (_con.Table<Folder>().Where((item) => item.Id == folderId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                _con.RunInTransaction(() =>
                {
                    var delectedPwds = _con
                        .Table<Password>()
                        .Where((item) => item.FolderId == folderId)
                        .Select((item) => new DelectedItem(item.Id, StorageType.Password))
                        .ToArray();
                    if (record)
                    {
                        _con.Insert(new DelectedItem(folderId, StorageType.Folder));
                        _con.InsertAll(delectedPwds);
                    }

                    foreach (var pwd in delectedPwds)
                    {
                        _con.Table<Icon>().Where((icon) => icon.Id == pwd.Id).Delete();
                        _con.Table<KeyValuePair>().Where(kvp => pwd.Id == kvp.SourceId).Delete();
                    }

                    _con.Table<Icon>().Where((item) => item.Id == folderId).Delete();
                    _con.Table<Folder>().Where((item) => item.Id == folderId).Delete();
                });
            });
        }

        public async Task<List<Folder>> GetFolders()
        {
            List<Folder> folders = null;
            await TaskExtend.Run(() =>
            {
                folders = _con.Table<Folder>().OrderBy((item) => item.Order).ToList();
            });
            return folders;
        }

        public async Task UpdateFolder(Folder folder, Icon icon = null)
        {
            await TaskExtend.Run(() =>
            {
                _con.RunInTransaction(() =>
                {
                    if (folder != null)
                    {
                        folder.LastEditTime = DateTime.Now;
                        _con.Update(folder);
                    }

                    if (icon != null)
                    {
                        _con.Update(icon);
                    }
                });
            });
        }
    }
}
