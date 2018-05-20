using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;

namespace XuHos.DTO
{
    public class UserAccountDTO
    {
        /// <summary>
        /// 账户ID
        /// </summary>
        public string AccountID { get; set; }

        ///// <summary>
        ///// 币种(0-人民币、1-港币、2-台币、3-美元)
        ///// </summary>
        //public int Currency { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }


        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal Available { get; set; }

        /// <summary>
        /// 冻结金额
        /// </summary>
        public decimal Freeze { get; set; }

        /// <summary>
        /// 账户状态(0-正常、1-冻结)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 总消费
        /// </summary>
        public decimal TotalConsume { get; set; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }


        public bool HavePayPassword { get; set; }

        public UserDTO User { get; set; }
    }

    public class CRMUserDTO
    {
        public string Mobile { get; set; }
    }

    public class PayPasswordDTO
    {
        public string CheckCode { get; set; }

        public string PayPassword { get; set; }

        public string PayPasswordConfirm { get; set; }
    }

    public class BindMobileDTO
    {
        public string CheckCode { get; set; }

        public string Mobile { get; set; }

        public string OutID { get; set; }
        public string OutUserID { get; set; }

    }

    public class UserAccountConfig
    {
        public decimal CashMaxLimit { get; set; }

        public decimal CashMinLimit { get; set; }

        public decimal RechargeMaxLimit { get; set; }

        public decimal RechargeMinLimit { get; set; }

    }
}
