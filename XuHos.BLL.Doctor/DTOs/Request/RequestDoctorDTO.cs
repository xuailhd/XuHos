using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生信息
    /// </summary>
    public class RequestDoctorDTO
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 医生英文信息
        /// </summary>
        public string DoctorEnName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public XuHos.Common.Enum.EnumUserGender Gender { get; set; }

        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        public XuHos.Common.Enum.EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public XuHos.Common.Enum.EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 资格证号
        /// </summary>
        public string CertificateNo { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 是否会诊（0-否、1-是）
        /// </summary>
        public bool IsConsultation { get; set; }

        /// <summary>
        /// 是否专家（0-否、1-是）
        /// </summary>
        public bool IsExpert { get; set; }

        /// <summary>
        /// 是否义诊断
        /// </summary>
        public bool IsFreeClinicr { get; set; }

        /// <summary>
        /// 特长
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string areaCode { get; set; }

        /// <summary>
        /// 医院ID（没有关联的医院ID默认为0）
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 科室ID（没有关联的科室ID默认为0）
        /// </summary>
        public string DepartmentID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 社会机构及职务
        /// </summary>
        public string Duties { get; set; }

        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-驳回）
        /// </summary>
        public int CheckState { get; set; }

        /// <summary>
        /// 签名图片URL
        /// </summary>
        public string SignatureURL { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 关注量
        /// </summary>
        public int FollowNum { get; set; }

        /// <summary>
        /// 诊断数量
        /// </summary>
        public int DiagnoseNum { get; set; }

        /// <summary>
        /// 咨询数量
        /// </summary>
        public int ConsultNum { get; set; }

        /// <summary>
        /// 远程会诊服务价格
        /// </summary>
        public decimal ConsulServicePrice { get; set; }

        
        /// <summary>
        /// 是否会诊（0-否、1-是）
        /// </summary>
        public bool IsFollowed { get; set; }

        

    }
}
