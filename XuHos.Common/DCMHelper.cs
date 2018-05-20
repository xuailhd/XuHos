using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common
{
    public static class DCMHelper
    {
        /// <summary>
        /// 获取病人病例号
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string,string> TagRead(string filePath)//不断读取所有tag 及其值 直到碰到图像数据 (7fe0 0010 )
        {
            try
            {
                Dictionary<string, string> tags = new Dictionary<string, string>();//dicom文件中的标签
                bool isLitteEndian = true;//是否小字节序（小端在前 、大端在前）
                BinaryReader dicomFile;//dicom文件流
                bool isExplicitVR = true;//有无VR
                UInt32 pixDatalen;//像素数据长度
                long pixDataOffset = 0;//像素数据开始位置
                UInt32 fileHeadLen;//文件头长度
                long fileHeadOffset;//文件数据开始位置

                dicomFile = new BinaryReader(File.OpenRead(filePath));
                dicomFile.BaseStream.Seek(128, SeekOrigin.Begin);
                if (new string(dicomFile.ReadChars(4)) != "DICM")
                {
                    return null;
                }
                bool enDir = false;
                int leve = 0;
                StringBuilder folderData = new StringBuilder();//该死的文件夹标签
                string folderTag = "";
                while (dicomFile.BaseStream.Position + 6 < dicomFile.BaseStream.Length)
                {
                    //读取tag
                    string tag = dicomFile.ReadUInt16().ToString("x4") + "," + dicomFile.ReadUInt16().ToString("x4");
                    
                    string VR = string.Empty;
                    UInt32 Len = 0;
                    //读取VR跟Len
                    //对OB OW SQ 要做特殊处理 先置两个字节0 然后4字节值长度
                    //------------------------------------------------------这些都是在读取VR一步被阻断的情况
                    if (tag.Substring(0, 4) == "0002")//文件头 特殊情况
                    {
                        VR = new string(dicomFile.ReadChars(2));

                        if (VR == "OB" || VR == "OW" || VR == "SQ" || VR == "OF" || VR == "UT" || VR == "UN")
                        {
                            dicomFile.BaseStream.Seek(2, SeekOrigin.Current);
                            Len = dicomFile.ReadUInt32();
                        }
                        else
                            Len = dicomFile.ReadUInt16();
                    }
                    else if (tag == "fffe,e000" || tag == "fffe,e00d" || tag == "fffe,e0dd")//文件夹标签
                    {
                        VR = "**";
                        Len = dicomFile.ReadUInt32();
                    }
                    else if (isExplicitVR == true)//有无VR的情况
                    {
                        VR = new string(dicomFile.ReadChars(2));

                        if (VR == "OB" || VR == "OW" || VR == "SQ" || VR == "OF" || VR == "UT" || VR == "UN")
                        {
                            dicomFile.BaseStream.Seek(2, SeekOrigin.Current);
                            Len = dicomFile.ReadUInt32();
                        }
                        else
                            Len = dicomFile.ReadUInt16();
                    }
                    else if (isExplicitVR == false)
                    {
                        VR = getVR(tag);//无显示VR时根据tag一个一个去找 真tm烦啊。
                        Len = dicomFile.ReadUInt32();
                    }
                    //判断是否应该读取VF 以何种方式读取VF
                    //-------------------------------------------------------这些都是在读取VF一步被阻断的情况
                    byte[] VF = { 0x00 };

                    if (tag == "7fe0,0010")//图像数据开始了
                    {
                        pixDatalen = Len;
                        pixDataOffset = dicomFile.BaseStream.Position;
                        dicomFile.BaseStream.Seek(Len, SeekOrigin.Current);
                        VR = "UL";
                        VF = BitConverter.GetBytes(Len);
                    }
                    else if ((VR == "SQ" && Len == UInt32.MaxValue) || (tag == "fffe,e000" && Len == UInt32.MaxValue))//靠 遇到文件夹开始标签了
                    {
                        if (enDir == false)
                        {
                            enDir = true;
                            folderData.Remove(0, folderData.Length);
                            folderTag = tag;
                        }
                        else
                        {
                            leve++;//VF不赋值
                        }
                    }
                    else if ((tag == "fffe,e00d" && Len == UInt32.MinValue) || (tag == "fffe,e0dd" && Len == UInt32.MinValue))//文件夹结束标签
                    {
                        if (enDir == true)
                        {
                            enDir = false;
                        }
                        else
                        {
                            leve--;
                        }
                    }
                    else
                        VF = dicomFile.ReadBytes((int)Len);

                    string VFStr;

                    VFStr = getVF(VR, VF);

                    //----------------------------------------------------------------针对特殊的tag的值的处理
                    //特别针对文件头信息处理
                    if (tag == "0002,0000")
                    {
                        fileHeadLen = Len;
                        fileHeadOffset = dicomFile.BaseStream.Position;
                    }
                    else if (tag == "0002,0010")//传输语法 关系到后面的数据读取
                    {
                        switch (VFStr)
                        {
                            case "1.2.840.10008.1.2.1\0"://显示little
                                isLitteEndian = true;
                                isExplicitVR = true;
                                break;
                            case "1.2.840.10008.1.2.2\0"://显示big
                                isLitteEndian = false;
                                isExplicitVR = true;
                                break;
                            case "1.2.840.10008.1.2\0"://隐式little
                                isLitteEndian = true;
                                isExplicitVR = false;
                                break;
                            default:
                                break;
                        }
                    }
                    for (int i = 1; i <= leve; i++)
                        tag = "--" + tag;
                    //------------------------------------数据搜集代码
                    if ((VR == "SQ" && Len == UInt32.MaxValue) || (tag == "fffe,e000" && Len == UInt32.MaxValue) || leve > 0)//文件夹标签代码
                    {
                        folderData.AppendLine(tag + "(" + VR + ")：" + VFStr);
                    }
                    else if (((tag == "fffe,e00d" && Len == UInt32.MinValue) || (tag == "fffe,e0dd" && Len == UInt32.MinValue)) && leve == 0)//文件夹结束标签
                    {
                        folderData.AppendLine(tag + "(" + VR + ")：" + VFStr);
                        if (!tags.ContainsKey(tag))
                            tags.Add(folderTag + "SQ", folderData.ToString());
                    }
                    else
                    {
                        if (!tags.ContainsKey(tag))
                            tags.Add(tag, VFStr);
                    }
                }
                return tags;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string getVR(string tag)
        {
            switch (tag)
            {
                case "0002,0000"://文件元信息长度
                    return "UL";
                    break;
                case "0002,0010"://传输语法
                    return "UI";
                    break;
                case "0002,0013"://文件生成程序的标题
                    return "SH";
                    break;
                case "0008,0005"://文本编码
                    return "CS";
                    break;
                case "0008,0008":
                    return "CS";
                    break;
                case "0008,1032"://成像时间
                    return "SQ";
                    break;
                case "0008,1111":
                    return "SQ";
                    break;
                case "0008,0020"://检查日期
                    return "DA";
                    break;
                case "0008,0060"://成像仪器
                    return "CS";
                    break;
                case "0008,0070"://成像仪厂商
                    return "LO";
                    break;
                case "0008,0080":
                    return "LO";
                    break;
                case "0010,0010"://病人姓名
                    return "PN";
                    break;
                case "0010,0020"://病人id
                    return "LO";
                    break;
                case "0010,0030"://病人生日
                    return "DA";
                    break;
                case "0018,0060"://电压
                    return "DS";
                    break;
                case "0018,1030"://协议名
                    return "LO";
                    break;
                case "0018,1151":
                    return "IS";
                    break;
                case "0020,0010"://检查ID
                    return "SH";
                    break;
                case "0020,0011"://序列
                    return "IS";
                    break;
                case "0020,0012"://成像编号
                    return "IS";
                    break;
                case "0020,0013"://影像编号
                    return "IS";
                    break;
                case "0028,0002"://像素采样1为灰度3为彩色
                    return "US";
                    break;
                case "0028,0004"://图像模式MONOCHROME2为灰度
                    return "CS";
                    break;
                case "0028,0006"://颜色值排列顺序 可能骨头重建那个的显示就是这个问题
                    return "US";
                    break;
                case "0028,0008"://图像的帧数
                    return "IS";
                    break;

                case "0028,0010"://row高
                    return "US";
                    break;
                case "0028,0011"://col宽
                    return "US";
                    break;
                case "0028,0100"://单个采样数据长度
                    return "US";
                    break;
                case "0028,0101"://实际长度
                    return "US";
                    break;
                case "0028,0102"://采样最大值
                    return "US";
                    break;
                case "0028,0103"://像素数据类型
                    return "US";
                    break;

                case "0028,1050"://窗位
                    return "DS";
                    break;
                case "0028,1051"://窗宽
                    return "DS";
                    break;
                case "0028,1052":
                    return "DS";
                    break;
                case "0028,1053":
                    return "DS";
                    break;
                case "0040,0008"://文件夹标签
                    return "SQ";
                    break;
                case "0040,0260"://文件夹标签
                    return "SQ";
                    break;
                case "0040,0275"://文件夹标签
                    return "SQ";
                    break;
                case "7fe0,0010"://像素数据开始处
                    return "OW";
                    break;
                default:
                    return "UN";
                    break;
            }
        }

        private static string getVF(string VR, byte[] VF)
        {
            if (VF.Length == 0)
                return "";
            string VFStr = string.Empty;
            //if (isLitteEndian == false)//如果是big字节序 先把数据翻转一下
            //    Array.Reverse(VF);
            switch (VR)
            {
                case "SS":
                    VFStr = BitConverter.ToInt16(VF, 0).ToString();
                    break;
                case "US":
                    VFStr = BitConverter.ToUInt16(VF, 0).ToString();

                    break;
                case "SL":
                    VFStr = BitConverter.ToInt32(VF, 0).ToString();

                    break;
                case "UL":
                    VFStr = BitConverter.ToUInt32(VF, 0).ToString();

                    break;
                case "AT":
                    VFStr = BitConverter.ToUInt16(VF, 0).ToString();

                    break;
                case "FL":
                    VFStr = BitConverter.ToSingle(VF, 0).ToString();

                    break;
                case "FD":
                    VFStr = BitConverter.ToDouble(VF, 0).ToString();

                    break;
                case "OB":
                    VFStr = BitConverter.ToString(VF, 0);
                    break;
                case "OW":
                    VFStr = BitConverter.ToString(VF, 0);
                    break;
                case "SQ":
                    VFStr = BitConverter.ToString(VF, 0);
                    break;
                case "OF":
                    VFStr = BitConverter.ToString(VF, 0);
                    break;
                case "UT":
                    VFStr = BitConverter.ToString(VF, 0);
                    break;
                case "UN":
                    VFStr = Encoding.Default.GetString(VF);
                    break;
                default:
                    VFStr = Encoding.Default.GetString(VF);
                    break;
            }
            return VFStr;
        }
    }
}
