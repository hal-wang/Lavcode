using GalaSoft.MvvmLight;

namespace Lavcode.Uwp.ViewModel
{
    public class KeyValuePairItem : ObservableObject
    {
        public KeyValuePairItem(PasswordDetailViewModel vm)
        {
            VM = vm;
        }

        public PasswordDetailViewModel VM { get; }

        public KeyValuePairItem(Lavcode.Model.KeyValuePair keyValuePair, PasswordDetailViewModel vm)
            : this(vm)
        {
            Key = keyValuePair.Key;
            Value = keyValuePair.Value;
        }

        private string _key = string.Empty;
        public string Key
        {
            get { return _key; }
            set { Set(ref _key, value); }
        }

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        private double _keyLength = 80;
        public double KeyLength
        {
            get { return _keyLength; }
            set { Set(ref _keyLength, value); }
        }
    }
}