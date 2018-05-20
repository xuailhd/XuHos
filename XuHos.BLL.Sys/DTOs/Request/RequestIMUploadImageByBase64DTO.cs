using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestUploadImageByBase64DTO
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Content { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FileSeq { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AccessKey { get; set; }

    }
}
