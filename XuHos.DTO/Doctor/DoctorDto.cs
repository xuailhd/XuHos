using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;

namespace XuHos.DTO
{

    public class DoctorBaseDTO
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
        /// 科室名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 特长
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// 职称名称
        /// </summary>
        public string TitleName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Title))
                {
                    return EnumHelper.ToDictionary<EnumDoctorTitle>()[this.Title];
                }
                return "";
            }
        }

        public string PhotoUrl { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoFullUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhotoUrl))
                {
                    return ImageBaseDTO.PaddingUrlPrefix("images/doctor/default.jpg");
                }
                else
                {
                    return ImageBaseDTO.PaddingUrlPrefix(PhotoUrl);
                }
            }
            set
            {
                PhotoUrl = value;
            }
        }
    }

    /// <summary>
    /// 医生信息
    /// </summary>
    public class DoctorDto : DoctorBaseDTO
    {
        public DoctorDto()
        {

        }
        
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
        /// 学历
        /// </summary>
        public string Education { get; set; }
        
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
        /// 科室信息
        /// </summary>
        public HospitalDepartmentDTO Department { get; set; }

        /// <summary>
        /// 医生服务信息
        /// </summary>
        public List<DoctorServiceDTO> DoctorServices;

        //谁挖的坑！！！
        //public List<DoctorServiceDTO> DoctorServices
        //{
        //    set { _doctorServices = value; }
        //    get
        //    {
        //        //补数据库缺少的服务类型数据
        //        if (_doctorServices == null)
        //            _doctorServices = new List<DoctorServiceDTO>();
        //        if (_doctorServices.FirstOrDefault(i => i.ServiceType == EnumDoctorServiceType.AudServiceType) == null)
        //            _doctorServices.Add(new DoctorServiceDTO() { ServiceType = EnumDoctorServiceType.AudServiceType });
        //        if (_doctorServices.FirstOrDefault(i => i.ServiceType == EnumDoctorServiceType.FamilyDoctor) == null)
        //            _doctorServices.Add(new DoctorServiceDTO() { ServiceType = EnumDoctorServiceType.FamilyDoctor });
        //        if (_doctorServices.FirstOrDefault(i => i.ServiceType == EnumDoctorServiceType.PicServiceType) == null)
        //            _doctorServices.Add(new DoctorServiceDTO() { ServiceType = EnumDoctorServiceType.PicServiceType });
        //        if (_doctorServices.FirstOrDefault(i => i.ServiceType == EnumDoctorServiceType.VidServiceType) == null)
        //            _doctorServices.Add(new DoctorServiceDTO() { ServiceType = EnumDoctorServiceType.VidServiceType });
        //        return _doctorServices;
        //    }
        //}

        /// <summary>
        /// 医院信息
        /// </summary>
        public HospitalDto Hospital { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDTO User { get; set; }

        /// <summary>
        /// 义诊信息
        /// </summary>
        public ResponseDoctorClinicReturnDTO DoctorClinic { get; set; }

        /// <summary>
        /// 外接系统数据关联
        /// </summary>
        public ConversationExternalDTO ConversationExternals { get; set; }


        /// <summary>
        /// 是否会诊（0-否、1-是）
        /// </summary>
        public bool IsFollowed { get; set; }

        /// <summary>
        /// 医院科室
        /// </summary>
        public partial class HospitalDepartmentDTO
        {

            /// <summary>
            /// 科室ID
            /// </summary>
            public string DepartmentID { get; set; }

            /// <summary>
            /// 医院ID
            /// </summary>
            public string HospitalID { get; set; }

            /// <summary>
            /// 科室名称
            /// </summary>
            public string DepartmentName { get; set; }

            /// <summary>
            /// 简介
            /// </summary>
            public string Intro { get; set; }

        }

        /// <summary>
        /// 医生价格服务
        /// </summary>
        public partial class DoctorServiceDTO
        {
            /// <summary>
            /// 服务ID
            /// </summary>
            public string ServiceID { get; set; }

            /// <summary>
            /// 医生ID
            /// </summary>
            public string DoctorID { get; set; }

            /// <summary>
            /// 服务类型
            /// </summary>
            public EnumDoctorServiceType ServiceType { get; set; }

            /// <summary>
            /// 服务开关(0-关闭服务、1-开启服务)
            /// </summary>
            public int ServiceSwitch { get; set; }

            /// <summary>
            /// 服务价格
            /// </summary>
            public decimal ServicePrice { get; set; }

            public bool HasSchedule { get; set; }
        }


        public class HospitalDto : Common.ImageBaseDTO
        {
            /// <summary>
            /// 医院ID
            /// </summary>
            public string HospitalID { get; set; }

            /// <summary>
            /// 医院名称
            /// </summary>
            public string HospitalName { get; set; }

            /// <summary>
            /// 简介
            /// </summary>
            public string Intro { get; set; }

            /// <summary>
            /// 许可证
            /// </summary>
            public string License { get; set; }


            string _LogoUrl;
            /// <summary>
            /// Logo地址
            /// </summary>
            public string LogoUrl
            {
                set
                {
                    _LogoUrl = value;
                }
                get
                {
                    return PaddingUrlPrefix(_LogoUrl);
                }
            }

            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 邮编
            /// </summary>
            public string PostCode { get; set; }

            /// <summary>
            /// 电话
            /// </summary>
            public string Telephone { get; set; }

            /// <summary>
            /// 电子邮箱
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// 面向智慧药房渠道ID（网络医院ID 或者 自己渠道 不使用智慧药房的药，为空）
            /// </summary>
            public string ChannelID { get; set; }

            /// <summary>
            /// 图片
            /// </summary>

            string _ImageUrl;
            /// <summary>
            /// Logo地址
            /// </summary>
            public string ImageUrl
            {
                set
                {
                    _ImageUrl = value;
                }
                get
                {

                    return PaddingUrlPrefix(_ImageUrl);
                }
            }

        }

    }
    


}
