using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.Entity;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class SysDictService
    {
        //for循环取值时，起到暂时缓存作用
        Dictionary<EnumDictType, List<SysDictDTO>> DictCahce = new Dictionary<EnumDictType, List<SysDictDTO>>();
        Dictionary<string, List<SysDictDTO>> DictCahceStr = new Dictionary<string, List<SysDictDTO>>();

        /// <summary>
        /// 根据分类查询字典所有项(为了方便取值，定义成索引器)
        /// </summary>
        /// <param name="dictType"></param>
        /// <returns></returns>
        public List<SysDictDTO> this[EnumDictType dictType]
        {
            get
            {
                if (!DictCahce.ContainsKey(dictType))
                    DictCahce[dictType] = GetDictByType(dictType);

                return DictCahce[dictType];
            }
        }
        public List<SysDictDTO> this[string dictType]
        {
            get
            {
                if (!DictCahceStr.ContainsKey(dictType))
                    DictCahceStr[dictType] = GetDictByType(dictType);

                return DictCahceStr[dictType];
            }
        }

        /// <summary>
        /// 查询字典名称
        /// </summary>
        /// <param name="dictType">字典分类</param>
        /// <param name="value">字典值</param>
        /// <returns></returns>
        public string GetDictName(EnumDictType dictType, string value)
        {
            var item = this[dictType].FirstOrDefault(i => i.DicCode == value);
            return item == null ? "" : item.CNName;
        }

        /// <summary>
        /// 根据分类查询字典项
        /// </summary>
        /// <param name="typeID">分类ID</param>
        /// <returns></returns>
        public List<SysDictDTO> GetDictByType(EnumDictType dictType)
        {
            string typeID = dictType.ToString();
            return GetDictByType(typeID);
        }

        public List<SysDictDTO> GetDictByType(string typeID)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.SysDicts
                            orderby item.OrderNo ascending
                            where item.IsDeleted == false && item.DictTypeID == typeID
                            select item;
                var list = query.ToList().Map<List<SysDict>, List<SysDictDTO>>();
                if (list == null)
                    list = new List<SysDictDTO>();

                return list;
            }
        }



        /// <summary>
        /// 用于绑定选择框的分类
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetDownlistByType(EnumDictType dictType)
        {
            string typeID = dictType.ToString();
            return GetDownlistByType(typeID);
        }
        public List<ResponseTextAndValue> GetDownlistByType(string dictType)
        {
            var list = this[dictType];
            var result = from i in list select new ResponseTextAndValue() { Text = i.CNName, Value = i.DicCode };
            return result.ToList();
        }
        /// <summary>
        /// 获取字典值对应的名称
        /// </summary>
        /// <param name="dictType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetTextByType(string dictType, string value)
        {
            return this[dictType].Where(w=>w.DicCode == value).Select(m=>m.CNName).FirstOrDefault();
        }

    }
}
