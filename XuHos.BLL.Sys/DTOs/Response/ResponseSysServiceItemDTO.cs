using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs
{
    public  class ResponseSysServiceItemDTO
    {
        public string ServiceItemID { get; set; }

        /// <summary>
        /// 服务类型ID
        /// </summary>
     
        public string ServiceTypeID { get; set; }

        /// <summary>
        /// 服务类别(兼容旧的逻辑 保留字段)
        /// </summary>
        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 服务项目名称
        /// </summary>
      
        public string ServiceItemName { get; set; }

        /// <summary>
        /// 服务项目编码
        /// </summary>
      
        public string ServiceItemCode { get; set; }

        /// <summary>
        /// 服务项目内容
        /// </summary>
       
        public string ServiceItemContent { get; set; }

        /// <summary>
        /// 是否医生服务（0-不是，1-是）
        /// </summary>
       
        public bool IsDoctorService { get; set; }

        /// <summary>
        /// 服务持续时间（分钟），-1无限制
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 金额上限
        /// </summary>
     
        public decimal AmountUp { get; set; }

        /// <summary>
        /// 金额下限
        /// </summary>
      
        public decimal AmountDown { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 定价类型（0-平台定价、1-机构定价、2-医生定价）
        /// </summary>
        public int PricingType { get; set; }

        /// <summary>
        /// 服务开关(0-关闭服务、1-开启服务)
        /// </summary>
      
        public bool ServiceSwitch { get; set; }

        /// <summary>
        /// 是否提佣（0-不提佣、1-提佣）
        /// </summary>
        public bool IsPushMoney { get; set; }

        /// <summary>
        /// 分佣模式（0-定额、1-百分比）
        /// </summary>
     
        public int CommissionType { get; set; }

        /// <summary>
        /// 佣金（模式为0时，10表示10元；模式为1时，10表示10%）
        /// </summary>
      
        public decimal Commission { get; set; }

        /// <summary>
        /// 是否能编辑（0-不能编辑、1-能编辑），默认1
        /// </summary>
        public bool EnableEdit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
      
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 提供者ID
        /// </summary>
        /// 
       
        public string ProviderID { get; set; }

        /// <summary>
        /// 外部关联ID
        /// </summary> 
         public string OutID { get; set; }

        public string ServiceItemLogoUrl { get; set; }
        /// <summary>
        /// Banner
        /// </summary>
        public string ServiceItemBannerUrl { get; set; }
    }
}
