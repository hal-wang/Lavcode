using Lavcode.Model;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordItem : IconItem
    {
        public PasswordItem(PasswordModel password, IconModel icon = null)
        {
            Set(password, icon);
        }

        public void Set(PasswordModel password, IconModel icon = null)
        {
            Password = password;
            Title = password.Title;
            Remark = password.Remark.Replace('\n', ' ').Replace('\r', ' ');

            if (icon == null)
            {
                //后台设置图标
                SetIcon(password.Icon);
            }
            else
            {
                Icon = icon;
            }
        }

        public PasswordModel Password { get; set; }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _remark = string.Empty;
        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value); }
        }
    }
}
