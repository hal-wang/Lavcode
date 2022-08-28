using HTools;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.Api
{
    public class FolderService : IFolderService
    {
        private readonly ConService _cs;
        public FolderService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task AddFolder(Folder folder, Icon icon)
        {
            await TaskExtend.Run(() =>
            {
                folder.LastEditTime = DateTime.Now;

                Connection.RunInTransaction(() =>
                {
                    if (folder.Order == 0)
                    {
                        int order = 1;
                        var maxOrder = Connection.Table<Folder>().OrderByDescending((item) => item.Order).Select((item) => item.Order).FirstOrDefault();
                        if (maxOrder != default)
                        {
                            order = maxOrder + 1;
                        }
                        folder.Order = order;
                    }

                    Connection.Insert(folder);
                    icon.Id = folder.Id;
                    Connection.Insert(icon);
                });
            });
        }

        public async Task DeleteFolder(string folderId, bool record = true)
        {
            if (Connection.Table<Folder>().Where((item) => item.Id == folderId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                Connection.RunInTransaction(() =>
                {
                    var delectedPwds = Connection
                        .Table<Password>()
                        .Where((item) => item.FolderId == folderId)
                        .Select((item) => new DelectedItem(item.Id, StorageType.Password))
                        .ToArray();
                    if (record)
                    {
                        Connection.Insert(new DelectedItem(folderId, StorageType.Folder));
                        Connection.InsertAll(delectedPwds);
                    }

                    foreach (var pwd in delectedPwds)
                    {
                        Connection.Table<Icon>().Where((icon) => icon.Id == pwd.Id).Delete();
                        Connection.Table<KeyValuePair>().Where(kvp => pwd.Id == kvp.SourceId).Delete();
                    }

                    Connection.Table<Icon>().Where((item) => item.Id == folderId).Delete();
                    Connection.Table<Folder>().Where((item) => item.Id == folderId).Delete();
                });
            });
        }

        public async Task<List<Folder>> GetFolders()
        {
            //_cs.PostAsync<GetFolderDto>("folder")
            List<Folder> folders = null;
            await TaskExtend.Run(() =>
            {
                folders = Connection.Table<Folder>().OrderBy((item) => item.Order).ToList();
            });
            return folders;
        }

        public async Task UpdateFolder(Folder folder, Icon icon = null)
        {
            await TaskExtend.Run(() =>
            {
                Connection.RunInTransaction(() =>
                {
                    if (folder != null)
                    {
                        folder.LastEditTime = DateTime.Now;
                        Connection.Update(folder);
                    }

                    if (icon != null)
                    {
                        Connection.Update(icon);
                    }
                });
            });
        }
    }
}
