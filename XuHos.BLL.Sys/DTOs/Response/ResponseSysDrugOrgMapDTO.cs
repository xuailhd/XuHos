using XuHos.Common.Enum;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class ResponseSysDrugOrgMapBaseDTO
    {
        /// <summary> 
        /// 药品机构映射ID
        /// </summary>
        public string DrugOrgMapID { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public string ChannelID { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 药品ID
        /// </summary>
        public string DrugID { get; set; }

        /// <summary>
        /// 药品编码
        /// </summary>
        public string DrugCode { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        public string DrugName { get; set; }

        /// <summary>
        /// 药品类型
        /// </summary>
        public EnumDrugType DrugType { get; set; }

        public string DrugTypeName
        {
            get
            {
                return this.DrugType.GetEnumDescript();
            }
        }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

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
        public bool IsPrescribed
        { get; set; }
        
        /// <summary>
        /// 底价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 渠道价格
        /// </summary>
        public decimal ChannelPrice { get; set; }

        public decimal? EditUnitPrice { get; set; }
    }
    public class ResponseSysDrugOrgMapDTO : ResponseSysDrugOrgMapBaseDTO
    {

        /// <summary>
        /// 拼音名称
        /// </summary>
        public string PinYinName { get; set; }
        
        /// <summary>
        /// 剂量
        /// </summary>
        public int Dose { get; set; }

        /// <summary>
        /// 剂量单位
        /// </summary>
        public string DoseUnit { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
                
        /// <summary>
        /// 是否停用 0没有停用,1停用
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 是否医保药品
        /// </summary>
        public bool IsInsured
        { get; set; }


        /// <summary>
        /// 是否含麻黄素
        /// </summary>
        public bool HasEphedrine
        {
            get; set;
        }

        /// <summary>
        /// 是否需要处方签
        /// </summary>
        public bool IsNeedSign
        {
            get; set;
        }
    }
}
