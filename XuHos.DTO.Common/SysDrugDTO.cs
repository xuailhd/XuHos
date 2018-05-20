using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;

namespace XuHos.DTO.Common
{
    [Serializable]
    public class SysDrugDTO
    {
        public SysDrugDTO()
        {
        }

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
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

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
        /// 药房名
        /// </summary>
        public string TopPharmacyName { get; set; }

        /// <summary>
        /// 药房药品ID
        /// </summary>
        public string PharmacyDrugID { get; set; }

        /// <summary>
        /// 是否停用 0没有停用,1停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否处方药
        /// </summary>
        public bool IsPrescribed { get; set; }

        /// <summary>
        /// 医保药品
        /// </summary>
        public bool IsInsured { get; set; }

        /// <summary>
        /// 是否含麻黄素
        /// </summary>
        public bool HasEphedrine { get; set; }

        /// <summary>
        /// 是否需要处方签
        /// </summary>
        public bool IsNeedSign { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal ChannelPrice { get; set; }

        /// <summary>
        /// 产地
        /// </summary>
        public string ProducePlace { get; set; }
        /// <summary>
        /// 导入状态 0-无错，1-有错
        /// </summary>
        public int ValidateState { get; set; }

        /// <summary>
        /// 导入时验证出错信息
        /// </summary>
        public string ImportErrorMsg { get; set; }

        /// <summary>
        /// 导入时验证出错信息
        /// </summary>
        public bool IsRepeat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DrugOrgMapID { get; set; }

        public string ChannelID { get; set; }
        public int ChannelVersionNo { get; set; }
        

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend
        {
            get; set;
        }
    }
}