using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Storage
{
    public interface IFileStorage
    {
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        Task WriteFile(string ShareName, string FilePath,byte[] buffer);

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        Task WriteFile(string ShareName, string FilePath, System.IO.Stream fileStream);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        Task<System.IO.Stream> OpenFile(string ShareName, string FilePath);

        /// <summary>
        /// 获取读取文件访问签名
        /// </summary>
        /// <param name="ShareName"></param>
        /// <param name="FilePath"></param>
        /// <param name="AccessStartTime"></param>
        /// <param name="AccessExpiryTime"></param>
        /// <returns></returns>
        string GetReadAccessSignature(string ShareName, string FilePath, DateTime? AccessStartTime = null, DateTime? AccessExpiryTime = null);

        void RemoveFile(string ShareName, string FilePath);

        bool Exists(string ShareName, string FilePath);
    }
}
