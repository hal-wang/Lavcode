﻿using Lavcode.Model;
using System;

namespace Lavcode.Service.BaseGit.Entities
{
    public class PasswordEntity : IEntity
    {
        public PasswordEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string FolderId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public int Order { get; set; }
        public DateTime LastEditTime { get; set; }

        public PasswordModel ToModel()
        {
            return new PasswordModel()
            {
                Id = Id,
                FolderId = FolderId,
                Title = Title,
                Value = Value,
                Remark = Remark,
                Order = Order
            };
        }

        public static PasswordEntity FromModel(PasswordModel model)
        {
            return new PasswordEntity()
            {
                Id = model.Id,
                FolderId = model.FolderId,
                Title = model.Title,
                Value = model.Value,
                Remark = model.Remark,
                Order = model.Order
            };
        }
    }
}