using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.Implements
{
    public class HRegionService
    {
        /// <summary>
        /// 省选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetProvinceDropdownList()
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Regions
                            orderby item.RegionID
                            where item.Level==1&& item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.Name,
                                Value = item.RegionID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 市选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetCityDropdownList(string provineId)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Regions
                            orderby item.RegionID
                            where item.ParentID == provineId && item.Level == 2 && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.Name,
                                Value = item.RegionID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 市选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetAreaDropdownList(string cityId)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Regions
                            orderby item.RegionID
                            where item.ParentID == cityId && item.Level == 3 && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.Name,
                                Value = item.RegionID
                            };
                return query.ToList();
            }
        }
    }
}
