using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
   public class SysBankDto
    {
        public SysBankDto()
        {
            BankID = string.Empty;
            BankName = string.Empty;
            Sort = 0;
        }

        /// <summary>
        /// 银行ID
        /// </summary>
        [Required]
        public string BankID { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Required]
        public string BankName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort { get; set; }
    }
}
