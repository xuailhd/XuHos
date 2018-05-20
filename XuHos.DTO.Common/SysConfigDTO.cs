using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO.Common
{

    public class SysConfigDTO
    {
        /// <summary>
        /// ÅäÖÃ½ÚKey
        /// </summary>
        public string ConfigKey { get; set; }

        /// <summary>
        /// ÅäÖÃ½ÚValue
        /// </summary>
        public string ConfigValue { get; set; }

        /// <summary>
        /// ±¸×¢
        /// </summary>
        public string Remark { get; set; }

    }
}
