using Lavcode.IService;
using Lavcode.Model;
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

        public async Task AddFolder(FolderModel folder, IconModel icon)
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

            await _con.CreateComment(folder);

            icon.Id = folder.Id;
            await _con.CreateComment(icon);
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
                await _con.DeleteComment<IconModel, string>(pwd.Id, (item1, item2) => item1.Id == item2);
                await _con.DeleteComment<KeyValuePairModel, string>(pwd.Id, (item1, item2) => item1.SourceId == item2);
            }

            await _con.DeleteComment<FolderModel, string>(folderId, (item1, item2) => item1.Id == item2);
            await _con.DeleteComment<IconModel, string>(folderId, (item1, item2) => item1.Id == item2);
        }

        public Task<List<FolderModel>> GetFolders()
        {
            var result = _con.FolderIssue.Comments.Select(item => item.Value).OrderBy(item => item.Order).ToList();
            return Task.FromResult(result);
        }

        public async Task UpdateFolder(FolderModel folder, IconModel icon = null)
        {
            if (folder != null)
            {
                folder.LastEditTime = DateTime.Now;
                await _con.UpdateComment(folder, (item1, item2) => item1.Id == item2.Id);
            }

            if (icon != null)
            {
                await _con.UpdateComment(icon, (item1, item2) => item1.Id == item2.Id);
            }
        }
    }
}
