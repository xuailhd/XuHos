using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO.Common
{

    public class SysDictDTO
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        public string DicID { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public string DictTypeID { get; set; }

        /// <summary>
        /// 字典编号
        /// </summary>
        public string DicCode { get; set; }

        /// <summary>
        /// 字典中文名
        /// </summary>
        public string CNName { get; set; }

        /// <summary>
        /// 字典英文名
        /// </summary>
        public string ENName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public SysDictTypeDTO SysDictType { get; set; }

    }

    public class SysDictTypeDTO
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public string DictTypeID { get; set; }

        /// <summary>
        /// 类型中文名
        /// </summary>
        public string DictTypeName { get; set; }
    }
}
