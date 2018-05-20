using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using NAudio;
using NAudio.Wave;

namespace XuHos.Common.Utility
{
    public class AudioHelper
    {

        /// <summary>
        /// 获取文件长度
        /// </summary>
        /// <param name="musicFilePath"></param>
        /// <returns></returns>
        public static double TotalSeconds(Stream ms)//musicFilePath是歌曲文件地址
        {
            try
            {
                using (var rdr = new NAudio.Wave.WaveFileReader(ms))
                {
                    using (var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr))
                    {

                        return wavStream.TotalTime.TotalSeconds;
                    }
                }                
            }
            catch
            {
                return 0.0;
            }
        }
    }
}
