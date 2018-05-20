using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public partial class ExamItemType : AuditableEntity
    {
        /// <summary>
        /// 检查类型ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ExamItemTypeID { get; set; }

        /// <summary>
        /// 父级检验检查ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ParentID { get; set; }

        /// <summary>
        /// 检验检查名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string ExamItemTypeName { get; set; }

        /// <summary>
        /// 检验检查英文名
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string ExamItemTypeEnName { get; set; }

        /// <summary>
        /// 拼音编码
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string PinYinCode { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Column(TypeName = "int")]
        public int DataType { get; set; }

        /// <summary>
        /// 正常值范围
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string RegularRange { get; set; }

        /// <summary>
        /// 是否在图表上显示
        /// </summary>
        [Column(TypeName = "bit")]
        public bool ShowOnChart { get; set; }

        /// <summary>
        /// DataType=2时的选项值
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(-1)]
        public string Options { get; set; }

        /// <summary>
        /// 输入的单位
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string InputUnit { get; set; }

        /// <summary>
        /// 输入的提示
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string InputHint { get; set; }

        /// <summary>
        /// 输入的统一单位
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string UnifiedUnit { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [Required]
        [Column(TypeName = "bit")]
        public bool IsRequired { get; set; }

    }
}
