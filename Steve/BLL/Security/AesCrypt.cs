using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Steve.BLL.Security
{
    class AesCrypt
    {
        private PasswordDeriveBytes passBytes;
        private byte[] Key;
        private byte[] IV;

        public AesCrypt()
        {
            passBytes = new PasswordDeriveBytes("1254fhgffh6543", Encoding.Default.GetBytes("dfgsdg2432534"));
            Key = passBytes.GetBytes(32);
            IV = passBytes.GetBytes(16);
        }

        public string EncryptAes(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key
                    , aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt
                        , encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(
                            csEncrypt))
                        {

                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return System.Text.Encoding.Default.GetString(encrypted);
        }
    }
}
