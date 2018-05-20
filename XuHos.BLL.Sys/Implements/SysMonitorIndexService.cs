using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DAL.EF;
using XuHos.Common.Cache;
using System.Linq.Dynamic;
using System.Reflection;
using System.Dynamic;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.Common.Cache.Keys;
using System.Text.RegularExpressions;

namespace XuHos.BLL.Sys.Implements
{
    public class SysMonitorIndexService
    {
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 修改指标数据
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Name"></param>
        /// <param name="OutID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool InsertAndUpdate(RequestSysMonitorIndexUpdateDTO request,bool NumberValuePlus=false)
        {
            using (DBEntities db = new DBEntities())
            {
                foreach (var item in request.Values)
                {
                    var model = db.SysMonitorIndexs.Where(a => a.Category == request.Category && a.Name == item.Key && a.OutID ==request.OutID).FirstOrDefault();
                    if (model == null)
                    {
                        db.SysMonitorIndexs.Add(new Entity.SysMonitorIndex()
                        {
                            Category = request.Category,
                            Name = item.Key,
                            OutID = request.OutID,
                            Value =string.IsNullOrEmpty(item.Value)?"-":item.Value,
                            ModifyTime = DateTime.Now
                        });
                    }
                    else
                    {
                        //数字值递增（保留原来的指标）
                        if (NumberValuePlus)
                        {
                            //原数据和现在的数据都是数字类型
                            if (IsNumeric(model.Value) && IsNumeric(item.Value))
                            {
                                model.Value = item.Value;
                            }
                            else
                            {
                                model.Value = item.Value;
                            }
                        }
                        else
                        {
                            model.Value = item.Value;
                        }

                        model.ModifyTime = DateTime.Now;
                        db.Update(model);
                    }
                }

                if (db.SaveChanges() > 0)
                {
                    //没用用到
                    //foreach (var item in request.Values)
                    //{
                    //    var cacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Sys_MonitorIndex, $"Category-{request.Category}:OutID-{request.OutID}:Name-{item.Key}");
                    //    item.Value.ToCache(cacheKey);
                    //}
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }

        /// <summary>
        /// 查询指标
        
        /// 日期：2017年7月6日
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public System.Data.DataTable QueryTable(string Category,DateTime StartDate,DateTime EndDate)
        {
            using (DBEntities db = new DBEntities())
            {
                var list = db.SysMonitorIndexs.Where(a => a.Category == Category && a.ModifyTime < EndDate && a.ModifyTime >= StartDate).ToList();

                System.Data.DataTable dt = new System.Data.DataTable();

                //按照外部记录编号进行分组
                var indexNameCategorys = list.GroupBy(a => a.Name);

                dt.Columns.Add("OutID");

                //创建表格列
                foreach (var category in indexNameCategorys)
                {
                    dt.Columns.Add(category.Key);
                }


                //按照外部记录编号进行分组
                var recordGroups = list.GroupBy(a => a.OutID).ToList();

                foreach (var recordGroup in recordGroups)
                {
                    var row = dt.NewRow();
                    row["OutID"] = recordGroup.Key;

                    foreach (var record in recordGroup)
                    {
                        row[record.Name] = record.Value;
                    }

                    dt.Rows.Add(row);
                }

                return dt;
            }
        }
    }
}
