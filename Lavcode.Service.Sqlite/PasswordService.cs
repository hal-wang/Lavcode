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

        public async Task AddPassword(PasswordModel password, IconModel icon, List<KeyValuePairModel> keyValuePairs = null)
        {
            await TaskExtend.Run(() =>
            {
                password.LastEditTime = DateTime.Now;

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
                    password.Id = Guid.NewGuid().ToString();
                }

                Connection.RunInTransaction(() =>
                {
                    var passwordEntity = PasswordEntity.FromModel(password);
                    Connection.Insert(passwordEntity);
                    icon.Id = passwordEntity.Id;
                    Connection.Insert(IconEntity.FromModel(icon));

                    if (keyValuePairs != null)
                    {
                        foreach (var kvp in keyValuePairs)
                        {
                            kvp.Id = default;
                            kvp.SourceId = password.Id;
                        }
                        var list = keyValuePairs.Select(item => KeyValuePairEntity.FromModel(item)).ToArray();
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

        public async Task<List<KeyValuePairModel>> GetKeyValuePairs(string passwordId)
        {
            List<KeyValuePairEntity> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<KeyValuePairEntity>().Where((item) => item.SourceId == passwordId).ToList();
            });
            return result.Select(item => item.ToModel()).ToList();
        }

        public async Task<List<PasswordModel>> GetPasswords(string folderId)
        {
            List<PasswordEntity> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<PasswordEntity>().Where((item) => item.FolderId == folderId).OrderBy((item) => item.Order).ToList();
            });
            return result.Select(item => item.ToModel()).ToList();
        }

        public async Task<List<PasswordModel>> GetPasswords()
        {
            List<PasswordEntity> result = null;
            await TaskExtend.Run(() =>
            {
                result = Connection.Table<PasswordEntity>().OrderBy((item) => item.FolderId).ThenBy((item) => item.Order).ToList();
            });
            return result.Select(item => item.ToModel()).ToList();
        }

        public async Task UpdatePassword(PasswordModel password, IconModel icon = null, List<KeyValuePairModel> keyValuePairs = null)
        {
            await TaskExtend.Run(() =>
            {
                password.LastEditTime = DateTime.Now;

                Connection.RunInTransaction(() =>
                {
                    Connection.Update(password);
                    if (icon != null)
                    {
                        Connection.Update(icon);
                    }

                    if (keyValuePairs != null)
                    {
                        Connection.Table<KeyValuePairEntity>().Where((item) => item.SourceId == password.Id).Delete();
                        foreach (var kvp in keyValuePairs)
                        {
                            kvp.Id = default;
                            kvp.SourceId = password.Id;
                        }
                        Connection.InsertAll(keyValuePairs);
                    }
                });
            });
        }
    }
}
