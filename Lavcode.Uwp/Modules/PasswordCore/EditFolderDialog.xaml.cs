using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Controls.Message;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class EditFolderDialog : ContentDialog, IResultDialog<bool>
    {
        public EditFolderDialog(Folder folder = null)
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
            DependencyProperty.Register("FolderName", typeof(string), typeof(EditFolderDialog), new PropertyMetadata(string.Empty));

        public Icon Icon
        {
            get { return (Icon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Icon), typeof(EditFolderDialog), new PropertyMetadata(null));


        public Folder Folder { get; private set; }

        private async void AddOrEditFolderDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if (Folder != null)
                {
                    var iconService = SimpleIoc.Default.GetInstance<IIconService>();
                    Icon = await iconService.GetIcon(Folder.Id);
                }
                else
                {
                    Icon = Icon.GetDefault(StorageType.Folder);
                }

                Teach();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        public bool Result { get; private set; }

        private async void Teach()
        {
            if (SettingHelper.Instance.AddFolderTaught)
            {
                return;
            }

            await PopupHelper.ShowTeachingTipAsync(FolderNameTextBox, "文件夹名称（新建文件夹 2/3）", "输入容易辨识的文件夹名称，易于管理密码");
            FolderName = "默认文件夹";
            await PopupHelper.ShowTeachingTipAsync(IconSelecter, "文件夹图标（新建文件夹 3/3）", "选择文件夹图标，可以选择内置图标、图片、路径图");
            SettingHelper.Instance.AddFolderTaught = true;

            await Save();
            this.Hide(true);
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

            var folderService = SimpleIoc.Default.GetInstance<IFolderService>();
            if (Folder == null) //添加
            {
                var folder = new Folder()
                {
                    Name = FolderName,
                };
                await folderService.AddFolder(folder, Icon);
                Folder = folder;
            }
            else //编辑
            {
                var folder = Folder.DeepClone();
                folder.Name = FolderName;

                await folderService.UpdateFolder(folder, Icon);
                Folder = folder;
            }
            return true;
        }
    }
}
