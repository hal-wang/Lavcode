using Lavcode.Uwp.Helpers.Sqlite;
using Lavcode.Uwp.Model;
using HTools;
using System.Collections.Generic;

namespace Lavcode.Uwp.View.FolderList
{
    public static class FolderHelper
    {
        public static async IAsyncEnumerable<FolderItem> GetFolderItems()
        {
            using var helper = new SqliteHelper();

            foreach (var folder in await helper.GetFolders())
            {
                FolderItem folderItem = null;
                await TaskExtend.Run(() =>
                {
                    folderItem = new FolderItem(folder);
                });
                yield return folderItem;
            }
        }
    }
}
