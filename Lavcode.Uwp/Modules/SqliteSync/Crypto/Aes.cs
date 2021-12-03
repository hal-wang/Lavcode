using System;
using System.Security.Cryptography;
using System.Text;

namespace Lavcode.Uwp.Modules.SqliteSync.Crypto
{
    public class Aes
    {
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
