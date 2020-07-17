using System;
using System.Security.Cryptography;
using System.Text;

namespace Hubery.Lavcode.Uwp.Cryptography
{
    public class Aes
    {
        private static byte[] Byte2Byte(byte[] data, byte[] key, byte[] iv, OperationType operationType, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (data == null)
            {
                return null;
            }

            System.Security.Cryptography.Aes aesAlg = System.Security.Cryptography.Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Padding = padding;

            ICryptoTransform cryptoTransform;
            if (operationType == OperationType.Encrypt)
                cryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            else
                cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
        }

        public static TOut Operate<TIn, TOut>(TIn data, byte[] key, byte[] iv, OperationType operationType, PaddingMode padding = PaddingMode.PKCS7) where TOut : class
        {
            byte[] bData;
            if (data is string s)
            {
                bData = Encoding.UTF8.GetBytes(s);
            }
            else if (data is byte[] b)
            {
                bData = b;
            }
            else
            {
                throw new NotSupportedException();
            }

            var value = Byte2Byte(bData, key, iv, operationType, padding);

            if (typeof(TOut) == typeof(string))
            {
                return Encoding.UTF8.GetString(value) as TOut;
            }
            else if (typeof(TOut) == typeof(byte[]))
            {
                return value as TOut;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static ICryptoTransform GetCryptoTransform(byte[] key, byte[] iv, OperationType operationType, PaddingMode padding = PaddingMode.PKCS7)
        {
            System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = padding;
            return operationType == OperationType.Encrypt ? aes.CreateEncryptor(aes.Key, aes.IV) : aes.CreateDecryptor(aes.Key, aes.IV);
        }
    }
}
