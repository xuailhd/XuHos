using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common
{
    public class StringEncrypt
    {
        private static string _encryptKey = "7ujm*IK<1qaz#EDC";//自定义加解密密钥

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="passwordString">密码</param>
        /// <param name="encryptFormat">加密格式：SHA1，MD5</param>
        /// <returns></returns>
        public static string EncryptPassword(string passwordString, string encryptFormat)
        {
            string ret = passwordString.Trim();
            if (encryptFormat.ToUpper() == "SHA1")
            {
                ret = EncryptWithSHA(passwordString);
            }
            else
            {
                ret = EncryptWithMD5(passwordString);
            }
            return ret;
        }

        public static string EncryptWithSHA(string data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder(40);
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }
            return returnValue.ToString();
        }
        public static string EncryptWithMD5(string data)
        {
            MD5 md5 = MD5.Create();
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder(32);
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }
            return returnValue.ToString();

        }

        public static string GetMD5(string Input, string Input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(Input_charset).GetBytes(Input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        #region 自定义加密与解密
        private static SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        static byte[] GetLegalKey(string Key)
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        static byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&9eu$935GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public static string Encrypt(string source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey(_encryptKey);
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public static string Decrypt(string source)
        {
            byte[] bytIn = Convert.FromBase64String(source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey(_encryptKey);
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        #endregion

        #region MD5 32位加密
        /// <summary>
        /// MD5 32位加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_32(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion


        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source">呆加密的串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string source, string key)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey(key);
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public static string Decrypt(string source, string key)
        {
            byte[] bytIn = Convert.FromBase64String(source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey(key);
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }


        public static string EncryptBase64(string source, string key)
        {
            var dd = Encoding.UTF8.GetBytes(source + EncryptWithMD5(key));
            return Convert.ToBase64String(dd);
        }

        public static string DecryptBase64(string source, string key)
        {
            var dec = Convert.FromBase64String(source);
            var re = Encoding.UTF8.GetString(dec);
            var md5key = EncryptWithMD5(key);
            int index = re.IndexOf(md5key);
            if (index > 0)
            {
                return re.Remove(index, md5key.Length);
            }
            return source;
        }
    }
}
