﻿using Lavcode.Model;
using SQLite;
using System;

namespace Lavcode.Service.Sqlite.Entities
{
    [Table("Password")]
    public class PasswordEntity
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string FolderId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(100)]
        public string Value { get; set; }

        [MaxLength(500)]
        public string Remark { get; set; }

        public int Order { get; set; }

        [Column("LastEditTime")]
        public DateTime UpdatedAt { get; set; }

        public PasswordModel ToModel()
        {
            return new PasswordModel()
            {
                Id = Id,
                FolderId = FolderId,
                Title = Title,
                Value = Value,
                Remark = Remark,
                Order = Order,
                UpdatedAt = UpdatedAt,
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
                Order = model.Order,
                UpdatedAt = model.UpdatedAt.DateTime
            };
        }
    }
}