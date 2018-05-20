using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace XuHos.Common.Utility
{
    /// <summary>
    /// 提供用于计算指定文件哈希值的方法
    /// <example>例如计算文件的MD5值:
    /// <code>
    ///   String hashMd5=HashHelper.ComputeMD5("MyFile.txt");
    /// </code>
    /// </example>
    /// <example>例如计算文件的SHA1值:
    /// <code>
    ///   String hashSha1 =HashHelper.ComputeSHA1("MyFile.txt");
    /// </code>
    /// </example>
    /// </summary>
    public sealed class HashHelper
    {
        /// <summary>
        ///  计算指定文件的MD5值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static String ComputeMD5(Stream stream)
        {
            String hashMD5 = String.Empty;



            //计算文件的MD5值
            System.Security.Cryptography.MD5 calculator = System.Security.Cryptography.MD5.Create();
            Byte[] buffer = calculator.ComputeHash(stream);
            calculator.Clear();
            //将字节数组转换成十六进制的字符串形式
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                stringBuilder.Append(buffer[i].ToString("x2"));
            }
            hashMD5 = stringBuilder.ToString();
            return hashMD5;
        }//ComputeMD5

        /// <summary>
        ///  计算指定文件的MD5值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static String ComputeMD5(byte[] stream)
        {
            String hashMD5 = String.Empty;



            //计算文件的MD5值
            System.Security.Cryptography.MD5 calculator = System.Security.Cryptography.MD5.Create();
            Byte[] buffer = calculator.ComputeHash(stream);
            calculator.Clear();
            //将字节数组转换成十六进制的字符串形式
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                stringBuilder.Append(buffer[i].ToString("x2"));
            }
            hashMD5 = stringBuilder.ToString();
            return hashMD5;
        }//ComputeMD5

        /// <summary>
        ///  计算指定文件的SHA1值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static String ComputeSHA1(Stream stream)
        {
            String hashSHA1 = String.Empty;
            //计算文件的SHA1值
            System.Security.Cryptography.SHA1 calculator = System.Security.Cryptography.SHA1.Create();
            Byte[] buffer = calculator.ComputeHash(stream);
            calculator.Clear();
            //将字节数组转换成十六进制的字符串形式
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                stringBuilder.Append(buffer[i].ToString("x2"));
            }
            hashSHA1 = stringBuilder.ToString();
            return hashSHA1;
        }
    }
}
