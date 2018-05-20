using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public class SysDrugOrgMapDTO
    {
        /// <summary>
        /// 药品机构映射ID
        /// </summary>
        public string DrugOrgMapID { get; set; }

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
        /// 拼音名称
        /// </summary>
        public string PinYinName { get; set; }

        /// <summary>
        /// 许可编号
        /// </summary>
        public string LicenseNo { get; set; }

        /// <summary>
        /// 剂量
        /// </summary>
        public int TotalDose { get; set; }

        /// <summary>
        /// 剂量单位
        /// </summary>
        public string DoseUnit { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }


        /// <summary>
        /// 药品类型
        /// </summary>
        public EnumDrugType DrugType { get; set; }

        /// <summary>
        /// 药房ID
        /// </summary>
        public string PharmacyID { get; set; }

        /// <summary>
        /// 分药房名
        /// </summary>
        public string PharmacyName { get; set; }

        /// <summary>
        /// 是否停用 0没有停用,1停用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否处方药
        /// </summary>
        public bool IsPrescribed
        { get; set; }

        /// <summary>
        /// 医保药品
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
        public bool IsNeedSign { get; set; }

        
        /// <summary>
        /// 药店销售价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 未生效的销售价
        /// </summary>
        public decimal? EditUnitPrice { get; set; }

        /// <summary>
        /// 渠道销售价
        /// </summary>
        public decimal ChannelPrice { get; set; }

        /// <summary>
        /// 药品来源
        /// </summary>
        public string DrugSourceName { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// 是否使用智慧药房药品
        /// </summary>
        public bool IsUseWisdom { get; set; }

        public string ProducePlace { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend
        {
            get; set;
        }
    }
}
