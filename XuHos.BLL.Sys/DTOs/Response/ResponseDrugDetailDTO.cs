using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class ResponseDrugDetailDTO
    {

        /// <summary>
        /// 药品编码
        /// </summary>
        public string DrugCode { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        public string DrugName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 药品有效期
        /// </summary>
        public string DrugExpiryDay { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNO { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 许可编号
        /// </summary>
        public string LicenseNo { get; set; }

        /// <summary>
        /// 是否处方药
        /// </summary>
        public bool IsPrescribed { get; set; }

        /// <summary>
        /// 主要成分
        /// </summary>
        public string ZYCF { get; set; }

        /// <summary>
        /// 成分
        /// </summary>
        public string CF { get; set; }

        /// <summary>
        /// 性状
        /// </summary>
        public string XZ { get; set; }

        /// <summary>
        /// 功能主治
        /// </summary>
        public string GNZZ { get; set; }

        /// <summary>
        /// 用法用量
        /// </summary>
        public string YFYL { get; set; }

        /// <summary>
        /// 不良反应
        /// </summary>
        public string BLFY { get; set; }

        /// <summary>
        /// 禁忌
        /// </summary>
        public string JJ { get; set; }

        /// <summary>
        /// 注意事项
        /// </summary>
        public string ZYSX { get; set; }

        /// <summary>
        ///  儿童用药
        /// </summary>
        public string ETYY { get; set; }

        /// <summary>
        ///  老年患者用药
        /// </summary>
        public string LNHZYY { get; set; }

        /// <summary>
        ///  孕妇及哺乳期妇女用药
        /// </summary>
        public string YFYY { get; set; }

        /// <summary>
        ///  药物相互作用
        /// </summary>
        public string YWXHZY { get; set; }

        /// <summary>
        ///  药物过量
        /// </summary>
        public string YWGL { get; set; }

        ///// <summary>
        /////  药理毒理
        ///// </summary>
        //public string YLDX { get; set; }

        /// <summary>
        ///  药代动力学
        /// </summary>
        public string YDDLX { get; set; }

        /// <summary>
        ///  贮藏
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        ///  包装
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        ///  有效期
        /// </summary>
        public string YXQ { get; set; }

        /// <summary>
        ///  执行标准
        /// </summary>
        public string ZXBZ { get; set; }
    }
}
