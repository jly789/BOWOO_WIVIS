
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace lib.Common.Crypto
{
    /// <summary>
    /// AES 표준 암/복호화 알고리즘 클래스
    /// </summary>
    public class CryptoManager
    {
        private static readonly string CryptoKey = "uw39if;foalfl%@$)(*()))$%!k90hlee1"; //ConfigurationManager.AppSettings.Get("ENCKEY");

        /// <summary>
        /// 암호화 메서드
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns>암호화된 문자열 반환</returns>
        static public string EncryptoString(string InputText)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

            byte[] Salt = Encoding.ASCII.GetBytes(CryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(CryptoKey, Salt);

            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Clear();

            string EcnryptedDate = Convert.ToBase64String(CipherBytes);

            return EcnryptedDate;
        }

        /// <summary>
        /// AES 암호화 메서드
        /// </summary>
        /// <param name="InputText">암호화된 문자열</param>
        /// <returns>복호화된 문자열 반환</returns>
        static public string DecryptString(string InputText)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] EncryptedDate = Convert.FromBase64String(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(CryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(CryptoKey, Salt);

            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(EncryptedDate);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            byte[] PlainText = new byte[EncryptedDate.Length];

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

            return DecryptedData;
        }
    }
}
