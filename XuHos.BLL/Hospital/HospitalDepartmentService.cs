using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO;

using XuHos.DTO.Common;
using XuHos.Entity;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL
{
    /// <summary>
    /// 医院相关业务处理
    /// </summary>
    public class HospitalDepartmentService : Common.CommonBaseService<XuHos.Entity.HospitalDepartment>
    {
        public HospitalDepartmentService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }
       
        /// <summary>
        /// 部门选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetDepartmentDropdownList(string hospitalId)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.HospitalDepartments
                            orderby item.ModifyTime descending
                            where item.IsDeleted == false && item.HospitalID == hospitalId
                            select new ResponseTextAndValue()
                            {
                                Text = item.DepartmentName,
                                Value = item.DepartmentID,
                                Data = new { CAT_NO = item.CAT_NO, PARENT_CAT_NO = item.PARENT_CAT_NO }
                            };
                return query.ToList();
            }
        }


    }
}