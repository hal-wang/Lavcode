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
    public class PasswordService : IPasswordService
    {
        private readonly ConService _cs;

        public PasswordService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task AddPassword(PasswordModel password)
        {
            var newPassword = await _cs.PostAsync<PasswordModel>("password", new CreatePasswordDto()
            {
                FolderId = password.FolderId,
                Remark = password.Remark,
                Title = password.Title,
                Value = password.Value,
                Icon = new UpsertIconDto()
                {
                    IconType = password.Icon.IconType,
                    Value = password.Icon.Value
                },
                KeyValuePairs = password.KeyValuePairs.Select(item => new UpsertKeyValuePairDto()
                {
                    Key = item.Key,
                    Value = item.Value
                }).ToList()
            });
            password.UpdatedAt = newPassword.UpdatedAt;
            password.Icon = newPassword.Icon;
            password.KeyValuePairs = newPassword.KeyValuePairs;
            password.Id = password.Id;
            password.Order = password.Order;
        }

        public async Task DeletePassword(string passwordId, bool record = true)
        {
            await _cs.DeleteAsync("password/:passwordId", param: new
            {
                passwordId
            });
        }

        public async Task<List<PasswordModel>> GetPasswords(string folderId)
        {
            var folders = await _cs.GetAsync<List<GetPasswordDto>>("password", query: new
            {
                folderId
            });
            return folders.Select(item => item.ToModel()).ToList();
        }

        public async Task<List<PasswordModel>> GetPasswords()
        {
            return await GetPasswords("");
        }

        public async Task UpdatePassword(PasswordModel password, bool skipIcon, bool skipKvp)
        {
            var newPassword = await _cs.PostAsync<PasswordModel>("password/:passwordId", new UpdatePasswordDto()
            {
                FolderId = password.FolderId,
                Remark = password.Remark,
                Title = password.Title,
                Value = password.Value,
                Order = password.Order,
                Icon = skipIcon ? null : new UpsertIconDto()
                {
                    IconType = password.Icon.IconType,
                    Value = password.Icon.Value
                },
                KeyValuePairs = skipKvp ? null : password.KeyValuePairs.Select(item => new UpsertKeyValuePairDto()
                {
                    Key = item.Key,
                    Value = item.Value
                }).ToList()
            },
            param: new
            {
                passwordId = password.Id,
            });
            password.UpdatedAt = newPassword.UpdatedAt;
            if (!skipIcon)
            {
                password.Icon = newPassword.Icon;
            }
            if (!skipKvp)
            {
                password.KeyValuePairs = newPassword.KeyValuePairs;
            }
        }
    }
}
