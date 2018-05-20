using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    public class RequestUserFileDTO
    {
        /// <summary>
        /// 文件地址
        /// </summary>
        [Required]
        public string FileUrl
        {
            
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        public string Remark { get; set; }
    }
}
