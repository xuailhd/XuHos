using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XuHos.Common
{
    public class ToolHelper
    {
        #region 自定义方法：XML序列化

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string XMLSerialize(Type type, object obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = true; // 忽略XML声明（如：<?xml version="1.0" encoding="UTF-8" ?>）
            settings.Encoding = Encoding.UTF8;  // 设置字符编码格式
            MemoryStream mem = new MemoryStream();

            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // 第一个参数是前缀，第二个参数是命名空间

                XmlSerializer xz = new XmlSerializer(type);
                xz.Serialize(writer, obj, ns);
                return Encoding.UTF8.GetString(mem.ToArray());
            }
        }

        #endregion

        #region 自定义方法：XML反序列化

        /// <summary>    
        /// XML反序列化    
        /// </summary>    
        /// <param name="type">类型</param>    
        /// <param name="xml">XML字符串</param>    
        /// <returns></returns>    
        public static object XMLDeserialize(Type type, string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
        }

        #endregion

        #region 自定义方法：Base64编码

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Base64Encoding(string text)
        {
            // 字符编码都使用UTF-8
            byte[] b = System.Text.Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(b);
        }

        #endregion

        #region 自定义方法：Base64解码

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Base64Decoding(string text)
        {
            // 字符编码都使用UTF-8
            byte[] b = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(b);
        }

        #endregion

        /// <summary>
        /// 去除guid中划线
        /// </summary>
        /// <param name="guidStr"></param>
        /// <returns></returns>
        public static string RemoveGuidLine(string guidStr)
        {
            guidStr = guidStr.Replace("-", "");
            return guidStr;
        }

        /// <summary>
        /// 从身份证中获取生日和性别
        /// </summary>
        /// <param name="idCard">身份证号</param>
        /// <param name="birthday">生日(1990-01-01)</param>
        /// <param name="sex">性别(0男，1女)</param>
        /// <returns>true获取成功，false获取失败</returns>
        public static bool GetBirthdaySexFromIdCard(string idCard, out string birthday, out string sex)
        {
            birthday = "";
            sex = "";
            if (string.IsNullOrEmpty(idCard) || (idCard.Length != 15 && idCard.Length != 18))
                return false;

            //处理18位的身份证号码从号码中得到生日和性别代码
            if (idCard.Length == 18)
            {
                birthday = idCard.Substring(6, 4) + "-" + idCard.Substring(10, 2) + "-" + idCard.Substring(12, 2);
                sex = idCard.Substring(14, 3);
            }
            //处理15位的身份证号码从号码中得到生日和性别代码
            if (idCard.Length == 15)
            {
                birthday = "19" + idCard.Substring(6, 2) + "-" + idCard.Substring(8, 2) + "-" + idCard.Substring(10, 2);
                sex = idCard.Substring(12, 3);
            }

            //性别代码为偶数是女性奇数为男性
            if (int.Parse(sex) % 2 == 0)
                sex = "1";//女
            else
                sex = "0";//男

            return true;
        }

        public static bool CheckPhoneNumber(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return false;
            }
            //这是以13* 14* 15* 17* 18* 开头的号码为准，其余全都不是
            string rightphone = @"^(1(3|4|5|7|8)[0-9])\d{8}$";
            Regex regex = new Regex(rightphone);
            return regex.IsMatch(mobile);
        }

        public static bool CheckBirthDay(string date)
        {
            try
            {
                if(string.IsNullOrEmpty(date))
                {
                    return false;
                }
                DateTime dt = Convert.ToDateTime(date);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }


        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char GetPinYinInitial(char c)
        {
            var s = ToolGood.Words.WordsHelper.GetAllPinYin(c)?.FirstOrDefault();
            return string.IsNullOrEmpty(s) ? c : s[0];
            //byte[] data = Encoding.GetEncoding("gb2312").GetBytes(c.ToString());
            //if (data.Length == 1)
            //{
            //    return c;
            //}
            //ushort code = (ushort)((data[0] << 8) + data[1]);
            //ushort[] areaCode = {45217,45253,45761,46318,46826,47010,47297,47614,48119,48119,49062,49324,
            //    49896,50371,50614,50622,50906,51387,51446,52218,52698,52698,52698,52980,53689,54481,55290};
            //for (int i = 0; i < 26; i++)
            //{
            //    if (code >= areaCode[i] && code <= (ushort)(areaCode[i + 1] - 1))
            //    {
            //        return (char)('A' + i);
            //    }
            //}
            //return c;
        }

        public static string GetPinYinCode(string Chineses)
        {
            var c = new char[Chineses.Length];
            for (int i = 0; i < Chineses.Length; i++)
            {
                c[i] = GetPinYinInitial(Chineses[i]);
            }
            return new string(c);
        }


        public static int GetAgeFromBirth(string birth)
        {
            int _Age;
            if (!string.IsNullOrEmpty(birth))
            {
                DateTime dt;

                if (DateTime.TryParse(birth, out dt) == true)
                {
                    _Age = DateTime.Now.Year - dt.Year - 1;
                    if (DateTime.Now.Month > dt.Month || (DateTime.Now.Month == dt.Month && DateTime.Now.Day >= dt.Day))
                    {
                        _Age += 1;
                    }
                    return _Age;
                }
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            try
            {
                int i = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
