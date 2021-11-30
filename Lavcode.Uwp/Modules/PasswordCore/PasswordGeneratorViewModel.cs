using GalaSoft.MvvmLight;
using System;
using System.Text;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordGeneratorViewModel : ViewModelBase
    {
        private readonly string _capitalLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly string _lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private readonly string _figures = "0123456789";
        private readonly string _punctuation = "`~!@#$%^&*()_+-={}|[]\\:\";'?,./";

        private bool _isLowercaseLettersEnable = true;
        public bool IsLowercaseLettersEnable
        {
            get { return _isLowercaseLettersEnable; }
            set { Set(ref _isLowercaseLettersEnable, value); }
        }

        private bool _isCapitalLettersEnable = true;
        public bool IsCapitalLettersEnable
        {
            get { return _isCapitalLettersEnable; }
            set { Set(ref _isCapitalLettersEnable, value); }
        }

        private bool _isFiguresEnable = true;
        public bool IsFiguresEnalbe
        {
            get { return _isFiguresEnable; }
            set { Set(ref _isFiguresEnable, value); }
        }

        private bool _isPunctuationEnable = false;
        public bool IsPunctuationEnable
        {
            get { return _isPunctuationEnable; }
            set { Set(ref _isPunctuationEnable, value); }
        }

        private int _length = 16;
        public int Length
        {
            get { return _length; }
            set { Set(ref _length, value); }
        }

        private string GetSourceStr()
        {
            StringBuilder result = new StringBuilder();
            var length = (IsCapitalLettersEnable ? _capitalLetter.Length : 1) * (IsFiguresEnalbe ? _figures.Length : 1) * (IsPunctuationEnable ? _punctuation.Length : 1) * (IsLowercaseLettersEnable ? _lowercaseLetters.Length : 1);

            if (IsCapitalLettersEnable)
            {
                for (int i = 0; i < length / _capitalLetter.Length; i++)
                {
                    result.Append(_capitalLetter);
                }
            }

            if (IsFiguresEnalbe)
            {
                for (int i = 0; i < length / _figures.Length; i++)
                {
                    result.Append(_figures);
                }
            }

            if (IsPunctuationEnable)
            {
                for (int i = 0; i < length / _punctuation.Length; i++)
                {
                    result.Append(_punctuation);
                }
            }

            if (IsLowercaseLettersEnable)
            {
                for (int i = 0; i < length / _lowercaseLetters.Length; i++)
                {
                    result.Append(_lowercaseLetters);
                }
            }

            return result.ToString();
        }

        public string GeneratePassword()
        {
            StringBuilder result = new StringBuilder();
            var sourceStr = GetSourceStr();
            Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result.Append(sourceStr[random.Next(0, sourceStr.Length)]);
            }
            return result.ToString();
        }
    }
}