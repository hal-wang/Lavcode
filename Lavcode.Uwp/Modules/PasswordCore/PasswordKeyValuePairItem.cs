using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordKeyValuePairItem : ObservableObject
    {
        public PasswordKeyValuePairItem(PasswordDetailViewModel vm)
        {
            VM = vm;
        }

        public PasswordDetailViewModel VM { get; }

        public PasswordKeyValuePairItem(Lavcode.Model.KeyValuePair keyValuePair, PasswordDetailViewModel vm)
            : this(vm)
        {
            Key = keyValuePair.Key;
            Value = keyValuePair.Value;
        }

        private string _key = string.Empty;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private double _keyLength = 80;
        public double KeyLength
        {
            get { return _keyLength; }
            set { SetProperty(ref _keyLength, value); }
        }
    }
}