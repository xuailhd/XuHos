using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    /// <summary>
    /// 就医记录
    /// </summary>
    public class ResponseMedicalRecordDTO
    {
        public string RecipeName { get; set; }
        public string RecipeType { get; set; }
        public List<ResponseMedicalRecordDiagnoseDTO> Diagnosis { get; set; }
        public List<ResponseMedicalRecordDrugDTO> Drugs { get; set; }
        public int TCMQuantity { get; set; }
        public string Usage { get; set; }

        /// <summary>
        /// 分几次服用
        /// </summary>
        public int Times{ get; set; }
        /// <summary>
        /// 每日几剂频率
        /// </summary>
        public int FreqTimes{ get; set; }

        /// <summary>
        /// 每几日
        /// </summary>

        public int FreqDay { get; set; }

        /// <summary>
        /// 几煎
        /// </summary>
        public int DecoctNum { get; set; }

        /// <summary>
        /// 煎药后水量
        /// </summary>
        public decimal DecoctTargetWater{ get; set; }

        /// <summary>
        /// 煎药前水量
        /// </summary>
        public decimal DecoctTotalWater{ get; set; }

        /// <summary>
        /// 中药制法
        /// </summary>
        public int BoilWay { get; set; }

        /// <summary>
        /// 制法 串 制法：水煎，{DecoctNum} 煎，清水 {DecoctTotalWater} 毫升，煎至 {DecoctTargetWater} 毫升
        /// </summary>
        public string BoilWayName { get; set; }

        /// <summary>
        /// 用法 串 用法：{Usage}，每 {recipe.FreqDay} 日 {FreqTimes} 剂，分 {Times} 次服
        /// </summary>
        public string UsageName { get; set; }
    }
    public class ResponseMedicalRecordDiagnoseDTO
    {
        public string DiseaseCode { get; set; }
        public string DiseaseName { get; set; }
    }
    public class ResponseMedicalRecordDrugDTO
    {
        public string DrugName { get; set; }
        public string Specification { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Frequency { get; set; }
        public string DrugRouteName { get; set; }

        /// <summary>
        /// sig 串 Sig: 3.00 ml 口服 QD
        /// </summary>
        public string SigName { get; set; }
    }
}
