using XuHos.DTO.Common;
using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    /// <summary>
    ///诊断和处方
    /// </summary>
    public class ResponseDoctorDiagnoseAndRecipeDTO
    {
        public ResponseDoctorDiagnoseAndRecipeDTO()
        {
            this.RecipeList = new List<DoctorRecipeFileDTO>();
            this.Sympton = "";
            this.PastMedicalHistory = "";
            this.PresentHistoryIllness = "";
            this.PhysicalExam = new List<UserPhysicalExamDTO>();
            this.ConsultationOpinions = new List<ConsultationOpinionDTO>();
        }


        [Required]
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 处方文件路径
        /// </summary>
        public string RecipeFileUrl { get; set; }

        public string MemberID { get; set; }

        public string UserID { get; set; }

        #region V3.2版本返回
        /// <summary>
        /// 主诉
        /// </summary>
        //[Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Sympton { get; set; }

        /// <summary>
        /// 既往病史
        /// </summary>
        //[Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PastMedicalHistory { get; set; }


        /// <summary>
        /// 现病史
        /// </summary>
        //[Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PresentHistoryIllness { get; set; }

        /// <summary>
        /// 医嘱
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Advised { get; set; }

        /// <summary>
        /// 初步诊断
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PreliminaryDiagnosis { get; set; }

        /// <summary>
        /// 过敏史
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AllergicHistory { get; set; }

        /// <summary>
        /// 生活史
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LifeHistory { get; set; }

        /// <summary>
        /// 个人史
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IndividualHistory { get; set; }

        /// <summary>
        /// 家族史
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FamilyMedicalHistory { get; set; }
        #endregion

        /// <summary>
        /// 物理检查
        /// </summary>
        [Required]
        public List<UserPhysicalExamDTO> PhysicalExam { get; set; }

        /// <summary>
        /// 病历
        /// </summary>
        public UserMedicalRecordDTO MedicalRecord { get; set; }

        /// <summary>
        /// 处方列表
        /// </summary>
        [Required]
        public List<DoctorRecipeFileDTO> RecipeList { get; set; }


        /// <summary>
        /// 分诊建议
        /// </summary>
        [Required]
        public List<ConsultationOpinionDTO> ConsultationOpinions { get; set; }
        public string OrgnazitionID { get; set; }

        /// <summary>
        /// 会诊建议
        /// </summary>
        public class ConsultationOpinionDTO
        {
            public ConsultationOpinionDTO()
            {
                this.Description = "";
                this.DoctorName = "";
            }

            public string Description
            { get; set; }

            public string DoctorName { get; set; }


        }

        /// <summary>
        /// 处方
        /// </summary>
        public class DoctorRecipeFileDTO
        {
            public DoctorRecipeFileDTO()
            {
                this.RecipeDate = "";
                this.OPDRegisterID = "";
                this.RecipeName = "";
                this.MemberID = "";
                this.OPDRegisterID = "";
                this.TCMQuantity = 1;
                this.Usage = "";
                this.RecipeType = EnumRecipeType.ChineseRecipe;
                this.RecipeFileStatus = EnumRecipeFileStatus.UnSign;
                this.RecipeFileID = "";
                this.DiagnoseList = new List<DoctorDiagnoseDetailDTO>();
                this.RecipeNo = "";


            }

            /// <summary>
            /// 处方ID
            /// </summary>
            public string RecipeFileID { get; set; }

            /// <summary>
            /// 医生ID
            /// </summary>
            public string DoctorID { get; set; }

            /// <summary>
            /// 成员ID
            /// </summary>
            public string MemberID { get; set; }

            /// <summary>
            /// 预约登记ID
            /// </summary>
            public string OPDRegisterID { get; set; }

            /// <summary>
            /// 处方文件路径
            /// </summary>
            public string RecipeFileUrl { get; set; }
            /// <summary>
            /// 处方时间
            /// </summary>
            public string RecipeDate { get; set; }

            /// <summary>
            /// 处方编号
            /// </summary>
            public string RecipeNo { get; set; }

            /// <summary>
            /// 处方名称
            /// </summary>
            [Required]
            public string RecipeName { get; set; }

            /// <summary>
            /// 每几日
            /// </summary>
            public int FreqDay { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            public EnumRecipeFileStatus RecipeFileStatus { get; set; }

            /// <summary>
            /// 处方类型
            /// </summary>
            [Required]
            public EnumRecipeType RecipeType { get; set; }

            /// <summary>
            /// 中药剂数
            /// </summary>
            [Required]
            public int TCMQuantity { get; set; }

            /// <summary>
            /// 用法
            /// </summary>
            public string Usage { get; set; }

            /// <summary>
            /// 处方金额
            /// </summary>
            public decimal Amount
            {
                get; set;
            }

            /// <summary>
            /// 中药制法
            /// </summary>
            public int BoilWay { get; set; }

            /// <summary>
            /// 代煎剂数
            /// </summary>
            public int ReplaceDose { get; set; }

            /// <summary>
            /// 代煎单价(默认2.5)
            /// </summary>
            public decimal ReplacePrice
            { get; set; }

            /// <summary>
            /// 几煎
            /// </summary>
            public int DecoctNum
            { get; set; }

            /// <summary>
            /// 煎药后水量
            /// </summary>
            public decimal DecoctTargetWater
            { get; set; }

            /// <summary>
            /// 煎药前水量
            /// </summary>
            public decimal DecoctTotalWater
            { get; set; }


            /// <summary>
            /// 每日几剂频率
            /// </summary>
            public int FreqTimes
            { get; set; }

            /// <summary>
            /// 分几次服用
            /// </summary>
            public int Times
            { get; set; }



            /// <summary>
            /// 药品详情
            /// </summary>
            [Required]
            public List<DoctorRecipeDrugDTO> Details { get; set; }

            /// <summary>
            /// 诊断列表
            /// </summary>

            [Required]
            public List<DoctorDiagnoseDetailDTO> DiagnoseList;


            public DTO.Platform.OrderDTO Order { get; set; }

            /// <summary>
            /// 处方制作方法
            /// </summary>
            public EnumRecipeMakingMethod RecipeMakingMethod { get; set; }
            public string TimeDoseUnit { get; set; }
            public int TimeDose { get; set; }
        }


        /// <summary>
        /// 单个处方
        /// </summary>
        public class DoctorRecipeDrugDTO
        {
            public DoctorRecipeDrugDTO()
            {
                this.Dose = 1;
                this.Quantity = 1;
                this.DrugRouteName = "";
                this.Frequency = "";
                this.Drug = new SysDrugDTO();
            }

            /// <summary>
            /// 剂量
            /// </summary>
            [Required]
            public decimal Dose { get; set; }

            /// <summary>
            /// 计费数量
            /// </summary>
            [Required]
            public int Quantity { get; set; }

            /// <summary>
            /// 用药途径
            /// </summary>
            public string DrugRouteName { get; set; }

            /// <summary>
            /// 脚注
            /// </summary>
            public string FootNote { get; set; }

            /// <summary>
            /// 用药频率
            /// </summary>
            public string Frequency { get; set; }

            [Required]
            public SysDrugDTO Drug { get; set; }

        }

        /// <summary>
        /// 单个诊断记录
        /// </summary>
        public class DoctorDiagnoseDetailDTO
        {
            public DoctorDiagnoseDetailDTO()
            {
                this.Description = "";
                this.Detail = new DiagnoseDetailDTO();
                this.IsPrimary = true;
            }

            /// <summary>
            /// 诊断详情
            /// </summary>
            public DiagnoseDetailDTO Detail { get; set; }

            /// <summary>
            /// 诊断描述
            /// </summary>
            public string Description { get; set; }


            public bool IsPrimary { get; set; }

            /// <summary>
            /// 诊断信息
            /// </summary>
            public class DiagnoseDetailDTO
            {
                public DiagnoseDetailDTO()
                {
                    this.DiseaseCode = "";
                    this.DiseaseName = "";
                    this.IsPrimary = true;
                    this.Description = "";
                    this.DiagnoseID = "";
                }

                /// <summary>
                /// 诊断ID
                /// </summary>      
                public string DiagnoseID { get; set; }


                /// <summary>
                /// 诊断编码
                /// </summary>
                [Required]
                public string DiseaseCode { get; set; }

                /// <summary>
                /// 诊断名称
                /// </summary>
                [Required]
                public string DiseaseName { get; set; }

                /// <summary>
                /// 诊断类型（1-中医诊断、2-西医诊断）
                /// </summary>
                [Required]
                public EnumDiagnoseType DiagnoseType { get; set; }

                /// <summary>
                /// 描述
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// 主要诊断
                /// </summary>
                [Required]
                public bool IsPrimary
                { get; set; }

            }

        }
    }




}
