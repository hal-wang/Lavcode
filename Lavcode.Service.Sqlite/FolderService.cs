using HTools;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.Sqlite.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class FolderService : IFolderService
    {
        private readonly ConService _cs;
        private SQLiteConnection Connection => _cs.Connection;

        public FolderService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task AddFolder(FolderModel folder)
        {
            await TaskExtend.Run(() =>
            {
                folder.UpdatedAt = DateTime.Now;

                Connection.RunInTransaction(() =>
                {
                    if (folder.Order == 0)
                    {
                        int order = 1;
                        var maxOrder = Connection.Table<FolderEntity>().OrderByDescending((item) => item.Order).Select((item) => item.Order).FirstOrDefault();
                        if (maxOrder != default)
                        {
                            order = maxOrder + 1;
                        }
                        folder.Order = order;
                    }

                    var folderEntity = FolderEntity.FromModel(folder);
                    folderEntity.Id = Guid.NewGuid().ToString();
                    Connection.Insert(folderEntity);
                    folder.Id = folderEntity.Id;
                    folder.Icon.Id = folderEntity.Id;
                    Connection.Insert(IconEntity.FromModel(folder.Icon));
                });
            });
        }

        public async Task DeleteFolder(string folderId, bool record = true)
        {
            if (Connection.Table<FolderEntity>().Where((item) => item.Id == folderId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                Connection.RunInTransaction(() =>
                {
                    var delectedPwds = Connection
                        .Table<PasswordEntity>()
                        .Where((item) => item.FolderId == folderId)
                        .Select((item) => new DelectedEntity(item.Id, StorageType.Password))
                        .ToArray();
                    if (record)
                    {
                        Connection.Insert(new DelectedEntity(folderId, StorageType.Folder));
                        Connection.InsertAll(delectedPwds);
                    }

                    foreach (var pwd in delectedPwds)
                    {
                        Connection.Table<IconEntity>().Where((icon) => icon.Id == pwd.Id).Delete();
                        Connection.Table<KeyValuePairEntity>().Where(kvp => pwd.Id == kvp.PasswordId).Delete();
                    }

                    Connection.Table<IconEntity>().Where((item) => item.Id == folderId).Delete();
                    Connection.Table<FolderEntity>().Where((item) => item.Id == folderId).Delete();
                });
            });
        }

        public async Task<List<FolderModel>> GetFolders()
        {
            List<FolderModel> folders = null;
            await TaskExtend.Run(() =>
            {
                folders = Connection
                    .Table<FolderEntity>()
                    .OrderBy((item) => item.Order)
                    .Select((item) => item.ToModel())
                    .Select(item =>
                    {
                        item.Icon = Connection.Table<IconEntity>().FirstOrDefault(icon => icon.Id == item.Id)?.ToModel();
                        return item;
                    })
                    .ToList();
            });
            return folders;
        }

        public async Task UpdateFolder(FolderModel folder, bool skipIcon)
        {
            await TaskExtend.Run(() =>
            {
                Connection.RunInTransaction(() =>
                {
                    if (folder != null)
                    {
                        folder.UpdatedAt = DateTime.Now;
                        Connection.Update(FolderEntity.FromModel(folder));
                    }

                    if (!skipIcon && folder.Icon != null)
                    {
                        folder.Icon.Id = folder.Id;
                        Connection.Update(IconEntity.FromModel(folder.Icon));
                    }
                });
            });
        }
    }
}
