using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    public partial class Region : AuditableEntity
    {
        public Region()
        {
           
        }
        
        /// <summary>
        /// 区域ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string RegionID { get; set; }


        /// <summary>
        /// 父ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ParentID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int Level { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "int")]
        public int HotLevel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column(TypeName = "int")]
        public int Order { get; set; }

    }
}
