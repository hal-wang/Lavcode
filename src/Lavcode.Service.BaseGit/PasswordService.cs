using HTools;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.BaseGit.Entities;
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

        public async Task AddPassword(PasswordModel password)
        {
            password.UpdatedAt = DateTime.Now;

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
                throw new Exception("不能重复添加");
            }

            var passwordEntity = PasswordEntity.FromModel(password);
            passwordEntity.Id = Guid.NewGuid().ToString();
            await _con.CreateComment(passwordEntity);
            password.Id = passwordEntity.Id;
            password.Icon.Id = passwordEntity.Id;
            await _con.CreateComment(IconEntity.FromModel(password.Icon));

            if (password.KeyValuePairs != null)
            {
                foreach (var kvp in password.KeyValuePairs)
                {
                    kvp.Id = Guid.NewGuid().ToString();
                    kvp.PasswordId = password.Id;
                    await _con.CreateComment(KeyValuePairEntity.FromModel(kvp));
                }
            }
        }

        public async Task DeletePassword(string passwordId, bool record = true)
        {
            if (_con.PasswordIssue.Comments.Where((item) => item.Value.Id == passwordId).Count() == 0)
            {
                return;
            }

            await _con.DeleteComment<PasswordEntity, string>(passwordId, (item1, item2) => item1.Id == item2);
            await _con.DeleteComment<IconEntity, string>(passwordId, (item1, item2) => item1.Id == item2);
            await _con.DeleteComment<KeyValuePairEntity, string>(passwordId, (item1, item2) => item1.PasswordId == item2);
        }

        public Task<List<PasswordModel>> GetPasswords(string folderId)
        {
            var result = _con
                .GetComments<PasswordEntity, string>(folderId, (item1, item2) => item1.FolderId == folderId)
                .Where(item => item.FolderId == folderId)
                .OrderBy((item) => item.FolderId).ThenBy((item) => item.Order)
                .Select(item => item.ToModel())
                .Select(item =>
                {
                    item.Icon = _con.IconIssue.Comments.FirstOrDefault(icon => icon.Value.Id == item.Id)?.Value?.ToModel();
                    item.KeyValuePairs = _con.KeyValuePairIssue.Comments.Where(kvp => kvp.Value.PasswordId == item.Id).Select(item => item.Value).Select(item => item.ToModel()).ToList();
                    return item;
                })
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<PasswordModel>> GetPasswords()
        {
            var result = _con.PasswordIssue.Comments
                .Select(item => item.Value)
                .Select(item => item.ToModel())
                .Select(item =>
                {
                    item.Icon = _con.IconIssue.Comments.FirstOrDefault(icon => icon.Value.Id == item.Id)?.Value?.ToModel();
                    item.KeyValuePairs = _con.KeyValuePairIssue.Comments.Where(kvp => kvp.Value.PasswordId == item.Id).Select(item => item.Value).Select(item => item.ToModel()).ToList();
                    return item;
                })
                .ToList();
            return Task.FromResult(result);
        }

        public async Task UpdatePassword(PasswordModel password, bool skipIcon, bool skipKvp)
        {
            password.UpdatedAt = DateTime.Now;
            await _con.UpdateComment(PasswordEntity.FromModel(password), (item1, item2) => item1.Id == item2.Id);
            if (!skipIcon && password.Icon != null)
            {
                password.Icon.Id = password.Id;
                await _con.UpdateComment(IconEntity.FromModel(password.Icon), (item1, item2) => item1.Id == item2.Id);
            }

            if (!skipKvp && password.KeyValuePairs != null)
            {
                await _con.DeleteComment<KeyValuePairEntity, string>(password.Id, (item1, item2) => item1.PasswordId == item2);
                foreach (var kvp in password.KeyValuePairs)
                {
                    kvp.Id = Guid.NewGuid().ToString();
                    kvp.PasswordId = password.Id;
                    await _con.CreateComment(KeyValuePairEntity.FromModel(kvp));
                }
            }
        }
    }
}
