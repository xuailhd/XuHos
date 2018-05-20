using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ExamItemTypeDTO
    {
        /// <summary>
        /// 检查类型ID
        /// </summary>
        public string ExamItemTypeID { get; set; }

        /// <summary>
        /// 父级检验检查ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 检验检查名称
        /// </summary>
        public string ExamItemTypeName { get; set; }

        /// <summary>
        /// 检验检查英文名
        /// </summary>
        public string ExamItemTypeEnName { get; set; }

        /// <summary>
        /// 拼音编码
        /// </summary>
        public string PinYinCode { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 正常值范围
        /// </summary>
        public string RegularRange { get; set; }

        /// <summary>
        /// 是否在图表上显示
        /// </summary>
        public bool ShowOnChart { get; set; }

        /// <summary>
        /// DataType=2时的选项值
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// 输入的单位
        /// </summary>
        public string InputUnit { get; set; }

        /// <summary>
        /// 输入的提示
        /// </summary>
        public string InputHint { get; set; }

        /// <summary>
        /// 输入的统一单位
        /// </summary>
        public string UnifiedUnit { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 子项目
        /// </summary>
        public List<ExamItemTypeDTO> SubExamItemTypes { get; set; }
    }
}
