using Force.DeepCloner;
using HTools.Uwp.Controls.Message;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Guide;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class FolderEditDialog : ContentDialog, IResultDialog<bool>
    {
        public FolderEditDialog(FolderModel folder = null)
        {
            Folder = folder;
            FolderName = folder == null ? string.Empty : folder.Name;
            Title = (folder == null ? "添加" : "编辑") + "文件夹";

            this.InitializeComponent();
            Loaded += AddOrEditFolderDialog_Loaded;
        }

        public string FolderName
        {
            get { return (string)GetValue(FolderNameProperty); }
            set { SetValue(FolderNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FolderName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderNameProperty =
            DependencyProperty.Register("FolderName", typeof(string), typeof(FolderEditDialog), new PropertyMetadata(string.Empty));

        public IconModel Icon
        {
            get { return (IconModel)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconModel), typeof(FolderEditDialog), new PropertyMetadata(null));


        public FolderModel Folder { get; private set; }

        private void AddOrEditFolderDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= AddOrEditFolderDialog_Loaded;
            try
            {
                if (Folder != null)
                {
                    Icon = Folder.Icon;
                }
                else
                {
                    Icon = IconModel.GetDefault(StorageType.Folder);
                }

                Teach();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        public bool Result { get; set; }

        private async void Teach()
        {
            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.AddFolderTaught),
                Total = 3,
                Index = 2,
                Type = "新建文件夹",
            }
            .Add(new GuideItem()
            {
                Title = "文件夹名称",
                Content = "输入容易辨识的文件夹名称，易于管理密码",
                Target = FolderNameTextBox,
            })
            .Add(() =>
            {
                FolderName = "默认文件夹";
            })
            .Add(new GuideItem()
            {
                Title = "文件夹图标",
                Content = "选择文件夹图标，可以选择内置图标、图片、路径图",
                Target = IconSelecter,
            })
            .Add(async () =>
            {
                await Save();
                this.Hide(true);
            })
            .End()
            .RunAsync();
        }

        private async void LayoutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;

            try
            {
                if (await Save())
                {
                    this.Hide(true);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private async Task<bool> Save()
        {
            if (string.IsNullOrEmpty(FolderName))
            {
                MessageHelper.ShowSticky(FolderNameTextBox, "名称不能为空", MessageType.Warning);
                return false;
            }

            await NetLoadingHelper.Invoke(async () =>
             {
                 var folderService = ServiceProvider.Services.GetService<IFolderService>();
                 if (Folder == null) //添加
                 {
                     var folder = new FolderModel()
                     {
                         Name = FolderName,
                         Icon = Icon
                     };
                     await folderService.AddFolder(folder);
                     Folder = folder;
                 }
                 else //编辑
                 {
                     var folder = Folder.DeepClone();
                     folder.Name = FolderName;
                     folder.Icon = Icon;

                     await folderService.UpdateFolder(folder, false);
                     Folder = folder;
                 }
             }, "正在保存");
            return true;
        }
    }
}
