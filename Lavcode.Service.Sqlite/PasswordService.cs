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
    public class PasswordService : IPasswordService
    {
        private readonly ConService _cs;
        private SQLiteConnection Connection => _cs.Connection;

        public PasswordService(IConService cs)
        {
            _cs = cs as ConService;
        }

        public async Task AddPassword(PasswordModel password)
        {
            await TaskExtend.Run(() =>
            {
                password.UpdatedAt = DateTime.Now;

                if (password.Order == 0)
                {
                    var order = Connection
                        .Table<PasswordEntity>()
                        .Where((item) => item.FolderId == password.FolderId)
                        .OrderByDescending((item) => item.Order)
                        .Take(1)
                        .Select((item) => item.Order)
                        .FirstOrDefault();

                    password.Order = order + 1;
                }

                // 如果有重复
                if (Connection.Table<PasswordEntity>().Where((item) => item.Id == password.Id).Count() > 0)
                {
                    throw new Exception("不能重复添加");
                }

                Connection.RunInTransaction(() =>
                {
                    var passwordEntity = PasswordEntity.FromModel(password);
                    passwordEntity.Id = Guid.NewGuid().ToString();
                    Connection.Insert(passwordEntity);
                    password.Icon.Id = passwordEntity.Id;
                    Connection.Insert(IconEntity.FromModel(password.Icon));

                    if (password.KeyValuePairs != null)
                    {
                        foreach (var kvp in password.KeyValuePairs)
                        {
                            kvp.Id = Guid.NewGuid().ToString();
                            kvp.SourceId = password.Id;
                        }
                        var list = password.KeyValuePairs.Select(item => KeyValuePairEntity.FromModel(item)).ToArray();
                        Connection.InsertAll(list);
                    }
                });
            });
        }

        public async Task DeletePassword(string passwordId, bool record = true)
        {
            if (Connection.Table<PasswordEntity>().Where((item) => item.Id == passwordId).Count() == 0)
            {
                return;
            }

            await TaskExtend.Run(() =>
            {
                Connection.RunInTransaction(() =>
                {
                    Connection.Table<PasswordEntity>().Where((item) => item.Id == passwordId).Delete();
                    Connection.Table<IconEntity>().Where((item) => item.Id == passwordId).Delete();
                    Connection.Table<KeyValuePairEntity>().Where(item => item.SourceId == passwordId).Delete();

                    if (record)
                    {
                        Connection.Insert(new DelectedEntity(passwordId, StorageType.Password));
                    }
                });
            });
        }

        public async Task<List<PasswordModel>> GetPasswords(string folderId)
        {
            List<PasswordModel> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection
                    .Table<PasswordEntity>()
                    .Where((item) => item.FolderId == folderId)
                    .OrderBy((item) => item.Order)
                    .Select((item) => item.ToModel())
                    .Select(item =>
                    {
                        item.Icon = Connection.Table<IconEntity>().FirstOrDefault(icon => icon.Id == item.Id)?.ToModel();
                        item.KeyValuePairs = Connection.Table<KeyValuePairEntity>().Where(kvp => kvp.SourceId == item.Id).Select(item => item.ToModel()).ToList();
                        return item;
                    })
                    .ToList();
            });
            return result;
        }

        public async Task<List<PasswordModel>> GetPasswords()
        {
            List<PasswordModel> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection
                    .Table<PasswordEntity>()
                    .OrderBy((item) => item.FolderId)
                    .ThenBy((item) => item.Order)
                    .Select((item) => item.ToModel())
                    .Select(item =>
                    {
                        item.Icon = Connection.Table<IconEntity>().FirstOrDefault(icon => icon.Id == item.Id)?.ToModel();
                        item.KeyValuePairs = Connection.Table<KeyValuePairEntity>().Where(kvp => kvp.SourceId == item.Id).Select(item => item.ToModel()).ToList();
                        return item;
                    })
                    .ToList();
            });
            return result;
        }

        public async Task UpdatePassword(PasswordModel password, bool skipIcon, bool skipKvp)
        {
            await TaskExtend.Run(() =>
            {
                password.UpdatedAt = DateTime.Now;

                Connection.RunInTransaction(() =>
                {
                    Connection.Update(password);
                    if (!skipIcon && password.Icon != null)
                    {
                        password.Icon.Id = password.Id;
                        Connection.Update(password.Icon);
                    }

                    if (!skipKvp && password.KeyValuePairs != null)
                    {
                        Connection.Table<KeyValuePairEntity>().Where((item) => item.SourceId == password.Id).Delete();
                        foreach (var kvp in password.KeyValuePairs)
                        {
                            kvp.Id = Guid.NewGuid().ToString();
                            kvp.SourceId = password.Id;
                        }
                        Connection.InsertAll(password.KeyValuePairs);
                    }
                });
            });
        }
    }
}
