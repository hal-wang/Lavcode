using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.BaseGit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.BaseGit
{
    public class FolderService : IFolderService
    {
        private readonly BaseGitConService _con;

        public FolderService(IConService cs)
        {
            _con = cs as BaseGitConService;
        }

        public async Task AddFolder(FolderModel folder)
        {
            folder.LastEditTime = DateTime.Now;
            if (folder.Order == 0)
            {
                int order = 1;
                var maxOrder = _con.FolderIssue.Comments.Select(item => item.Value).OrderByDescending((item) => item.Order).Select((item) => item.Order).FirstOrDefault();
                if (maxOrder != default)
                {
                    order = maxOrder + 1;
                }
                folder.Order = order;
            }

            var folderEntity = FolderEntity.FromModel(folder);
            await _con.CreateComment(folderEntity);

            folder.Icon.Id = folderEntity.Id;
            await _con.CreateComment(IconEntity.FromModel(folder.Icon));
        }

        public async Task DeleteFolder(string folderId, bool record = true)
        {
            if (_con.FolderIssue.Comments.Select(item => item.Value).Where((item) => item.Id == folderId).Count() == 0)
            {
                return;
            }

            var delectedPwds = _con
                .PasswordIssue.Comments
                .Where((item) => item.Value.FolderId == folderId)
                .Select((item) => item.Value)
                .ToArray();

            foreach (var pwd in delectedPwds)
            {
                await _con.DeleteComment<IconEntity, string>(pwd.Id, (item1, item2) => item1.Id == item2);
                await _con.DeleteComment<KeyValuePairEntity, string>(pwd.Id, (item1, item2) => item1.SourceId == item2);
            }

            await _con.DeleteComment<IconEntity, string>(folderId, (item1, item2) => item1.Id == item2);
        }

        public Task<List<FolderModel>> GetFolders()
        {
            var result = _con.FolderIssue.Comments
                .Select(item => item.Value)
                .Select(item => item.ToModel())
                .Select(item =>
                {
                    item.Icon = _con.IconIssue.Comments.FirstOrDefault(icon => icon.Value.Id == item.Id)?.Value?.ToModel();
                    return item;
                })
                .OrderBy(item => item.Order)
                .ToList();
            return Task.FromResult(result);
        }

        public async Task UpdateFolder(FolderModel folder, bool skipIcon)
        {
            if (folder != null)
            {
                folder.LastEditTime = DateTime.Now;
                await _con.UpdateComment(FolderEntity.FromModel(folder), (item1, item2) => item1.Id == item2.Id);
            }

            if (!skipIcon && folder.Icon != null)
            {
                folder.Icon.Id = folder.Id;
                await _con.UpdateComment(IconEntity.FromModel(folder.Icon), (item1, item2) => item1.Id == item2.Id);
            }
        }
    }
}
