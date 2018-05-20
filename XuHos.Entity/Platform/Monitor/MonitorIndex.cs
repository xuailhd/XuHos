using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    /// <summary>
    /// 监控用户看诊指标
    /// </summary>
    public class SysMonitorIndex
    {
        public SysMonitorIndex()
        {
        }

        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string SysMonitorID { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OutID { get; set; }

        /// <summary>
        /// 指标分类
        /// </summary>
        [ Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Category { get; set; }



        /// <summary>
        /// 指标名称
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 指标数据
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(1000)]
        public string Value { get; set;}


        [Required]
        public DateTime ModifyTime { get; set; }
    }
}
