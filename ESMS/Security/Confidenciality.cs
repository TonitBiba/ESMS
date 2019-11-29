using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ESMS.Security
{
    public class Confidenciality
    {
        private static AesCryptoServiceProvider aesCrypto;

        public static string Enkrypt(int objectToBeEncrypted)
        {
            string iv = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AesIV"];
            string key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AesKey"];
            aesCrypto = new AesCryptoServiceProvider();
            aesCrypto.Mode = CipherMode.CBC;
            aesCrypto.Padding = PaddingMode.PKCS7;
            aesCrypto.IV = Convert.FromBase64String(iv);
            aesCrypto.Key = Convert.FromBase64String(key);

            try
            {
                ICryptoTransform cryptoTransform = aesCrypto.CreateEncryptor();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(objectToBeEncrypted.ToString());
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static string EncryptString(string objectToBeEncrypted)
        {
            string iv = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AesIV"];
            string key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AesKey"];
            aesCrypto = new AesCryptoServiceProvider();
            aesCrypto.Mode = CipherMode.CBC;
            aesCrypto.Padding = PaddingMode.PKCS7;
            aesCrypto.IV = Convert.FromBase64String(iv);
            aesCrypto.Key = Convert.FromBase64String(key);

            try
            {
                ICryptoTransform cryptoTransform = aesCrypto.CreateEncryptor();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(objectToBeEncrypted);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static T Decrypt<T>(string objectToBeDecrypted)
        {
            objectToBeDecrypted = objectToBeDecrypted.Replace(" ", "+");
            string iv = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AesIV"];
            string key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AesKey"];
            aesCrypto = new AesCryptoServiceProvider();
            aesCrypto.Mode = CipherMode.CBC;
            aesCrypto.Padding = PaddingMode.PKCS7;
            aesCrypto.IV = Convert.FromBase64String(iv);
            aesCrypto.Key = Convert.FromBase64String(key);

            try
            {
                ICryptoTransform cryptoTransform = aesCrypto.CreateDecryptor();

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(objectToBeDecrypted)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            if(typeof(T) == typeof(int))
                            {
                                var txt = streamReader.ReadToEnd();
                                return (T)Convert.ChangeType(Convert.ToInt32(txt), typeof(T));
                            }
                            else
                            {
                                return (T)Convert.ChangeType(streamReader.ReadToEnd(), typeof(T));
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return (T)Convert.ChangeType(0, typeof(T));
            }
        }
    }
}
