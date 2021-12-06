using HTools;
using Lavcode.IService;
using Lavcode.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.BaseGit
{
    public class PasswordService : IPasswordService
    {
        private readonly BaseGitConService _con;

        public PasswordService(IConService cs)
        {
            _con = cs as BaseGitConService;
        }

        public async Task AddPassword(Password password, Icon icon, List<KeyValuePair> keyValuePairs = null)
        {
            password.LastEditTime = DateTime.Now;

            if (password.Order == 0)
            {
                var order = _con
                    .PasswordIssue.Comments
                    .Select(item => item.Value)
                    .Where((item) => item.FolderId == password.FolderId)
                    .OrderByDescending((item) => item.Order)
                    .Take(1)
                    .Select((item) => item.Order)
                    .FirstOrDefault();

                password.Order = order + 1;
            }

            // 如果有重复
            if (_con.PasswordIssue.Comments.Where((item) => item.Value.Id == password.Id).Count() > 0)
            {
                password.SetNewId();
            }

            await _con.CreateComment(password);

            icon.Id = password.Id;
            await _con.CreateComment(icon);
            if (keyValuePairs != null)
            {
                foreach (var kvp in keyValuePairs)
                {
                    kvp.Id = default;
                    kvp.SourceId = password.Id;
                    await _con.CreateComment(kvp);
                }
            }
        }

        public async Task DeletePassword(string passwordId, bool record = true)
        {
            if (_con.PasswordIssue.Comments.Where((item) => item.Value.Id == passwordId).Count() == 0)
            {
                return;
            }

            await _con.DeleteComment<Password, string>(passwordId, (item1, item2) => item1.Id == item2);
            await _con.DeleteComment<Icon, string>(passwordId, (item1, item2) => item1.Id == item2);
            await _con.DeleteComment<KeyValuePair, string>(passwordId, (item1, item2) => item1.SourceId == item2);

            if (record)
            {
                await _con.CreateComment(new DelectedItem(passwordId, StorageType.Password));
            }

        }

        public Task<List<KeyValuePair>> GetKeyValuePairs(string passwordId)
        {
            var result = _con.KeyValuePairIssue.Comments.Where(item => item.Value.SourceId == passwordId).Select(item => item.Value).ToList();
            return Task.FromResult(result);
        }

        public Task<List<Password>> GetPasswords(string folderId)
        {
            var result = _con
                .GetComments<Password, string>(folderId, (item1, item2) => item1.FolderId == folderId)
                .Where(item => item.FolderId == folderId)
                .OrderBy((item) => item.FolderId).ThenBy((item) => item.Order)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<Password>> GetPasswords()
        {
            var result = _con.PasswordIssue.Comments.Select(item => item.Value).ToList();
            return Task.FromResult(result);
        }

        public async Task UpdatePassword(Password password, Icon icon = null, List<KeyValuePair> keyValuePairs = null)
        {
            password.LastEditTime = DateTime.Now;
            await _con.UpdateComment(password, (item1, item2) => item1.Id == item2.Id);
            if (icon != null)
            {
                await _con.UpdateComment(icon, (item1, item2) => item1.Id == item2.Id);
            }

            if (keyValuePairs != null)
            {
                await _con.DeleteComment<KeyValuePair, string>(password.Id, (item1, item2) => item1.SourceId == item2);
                foreach (var kvp in keyValuePairs)
                {
                    kvp.Id = default;
                    kvp.SourceId = password.Id;
                    await _con.CreateComment(kvp);
                }
            }
        }
    }
}
