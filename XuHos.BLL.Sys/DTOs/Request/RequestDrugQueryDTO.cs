using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestDrugQueryDTO
    {
        public RequestDrugQueryDTO()
        {
            DrugType = EnumDrugType.Chinese;
            Keyword = "";
            CurrentPage = 1;
            PageSize = 10;
            UserID = "";
            PharmacyID = "";
        }

        public EnumDrugType DrugType { get; set; }

        public string Keyword { get; set; }

        public DateTime? LastUpdTime;

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public string UserID { get; set; }

        /// <summary>
        /// 药店ID
        /// </summary>
        public string PharmacyID { get; set; }
    }
}