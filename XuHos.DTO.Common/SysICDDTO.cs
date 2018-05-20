using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO.Common
{

    public class SysICDDTO
    {
        /// <summary>
        /// ICDID
        /// </summary>
        public string ICDID { get; set; }

        /// <summary>
        /// º≤≤°±‡∫≈
        /// </summary>
        public string DiseaseCode { get; set; }

        /// <summary>
        /// º≤≤°√˚≥∆
        /// </summary>
        public string DiseaseName { get; set; }

        /// <summary>
        /// º≤≤°”¢Œƒ√˚≥∆
        /// </summary>
        public string DiseaseEnName { get; set; }

        /// <summary>
        /// ∆¥“Ù±‡¬Î
        /// </summary>
        public string PinYinCode { get; set; }

        /// <summary>
        /// ¿‡–Õ±‡¬Î
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// º≤≤°¿‡–Õ(1-÷–“Ωº≤≤°°¢2-Œ˜“Ωº≤≤°)
        /// </summary>
        public XuHos.Common.Enum.EnumDiagnoseType ICDType { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
