using XuHos.Common.Enum;
using XuHos.DTO.Common;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{
    /// <summary>
    /// 医生信息
    /// </summary>
    public class ResponseUserDoctorDTO
    {
        public ResponseUserDoctorDTO()
        {

        }

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
        /// 是否会诊（0-否、1-是）
        /// </summary>
        public bool IsFollowed { get; set; }

        /// <summary>
        /// 评价数量
        /// </summary>
        public int EvaluationNum { get; set; }

        /// <summary>
        /// 服务次数
        /// </summary>
        public int ServiceNum { get; set; }

        /// <summary>
        /// 远程会诊服务价格
        /// </summary>
        public decimal ConsulServicePrice { get; set; }

        /// <summary>
        /// 科室信息
        /// </summary>
        public ResponseHospitalDepartmentDTO Department { get; set; }

        /// <summary>
        /// 医生服务信息
        /// </summary>
        public List<ResponseDoctorServiceDTO> DoctorServices;

        /// <summary>
        /// 医院信息
        /// </summary>
        public ResponseHospitalDto Hospital { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ResopnseUserDTO User { get; set; }


        public ResponseDoctorClinicDTO DoctorClinic { get; set; }

        /// <summary>
        /// 服务总收入
        /// </summary>
        public decimal ServiceIncome { get; set; }




        /// <summary>
        /// 医院科室
        /// </summary>
        public partial class ResponseHospitalDepartmentDTO
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
        public partial class ResponseDoctorServiceDTO
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


        public class ResponseHospitalDto : ImageBaseDTO
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


        public class ResopnseUserDTO : ImageBaseDTO
        {

            /// <summary>
            /// 用户ID
            /// </summary>
            public string UserID { get; set; }

            /// <summary>
            /// 自己关系的 memberID
            /// </summary>
            public string MemberID { get; set; }

            /// <summary>
            /// 账号
            /// </summary>
            public string UserAccount { get; set; }

            /// <summary>
            /// 用户中文名
            /// </summary>
            public string UserCNName { get; set; }

            /// <summary>
            /// 用户英文名
            /// </summary>
            public string UserENName { get; set; }

            /// <summary>
            /// 用户类型(0-管理员、1-患者、2-医生)
            /// </summary>
            public EnumUserType UserType { get; set; }

            /// <summary>
            /// 手机号码
            /// </summary>
            public string Mobile { get; set; }

            /// <summary>
            /// 电子邮箱
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// 登录密码
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// 支付密码
            /// </summary>
            public string PayPassword { get; set; }

            public string _PhotoUrl { get; set; }

            /// <summary>
            /// 头像路径
            /// </summary>
            public string PhotoUrl
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_PhotoUrl))
                    {
                        if (UserType == EnumUserType.Doctor)
                        {
                            return PaddingUrlPrefix("images/doctor/default.jpg");
                        }
                        else
                        {
                            return PaddingUrlPrefix("images/doctor/unknow.jpg");
                        }

                    }
                    else
                    {
                        return PaddingUrlPrefix(_PhotoUrl);
                    }
                }
                set
                {
                    _PhotoUrl = value;
                }
            }



            /// <summary>
            /// 积分
            /// </summary>
            public int Score { get; set; }

            /// <summary>
            /// 星级
            /// </summary>
            public int Star { get; set; }

            /// <summary>
            /// 评价总数
            /// </summary>
            public int Comment { get; set; }

            /// <summary>
            /// 好评数
            /// </summary>
            public int Good { get; set; }

            /// <summary>
            /// 粉丝数
            /// </summary>
            public int Fans { get; set; }

            /// <summary>
            /// 等级
            /// </summary>
            public int Grade { get; set; }

            /// <summary>
            /// 认证
            /// </summary>
            public int Checked { get; set; }

            /// <summary>
            /// 注册时间
            /// </summary>
            public DateTime RegTime { get; set; }

            /// <summary>
            /// 注销时间
            /// </summary>
            public DateTime CancelTime { get; set; }

            /// <summary>
            /// 用户状态(0-正常、1-冻结、2-销户)
            /// </summary>
            public EnumUserState UserState { get; set; }

            /// <summary>
            /// 用户级别(0-普通用户、1-会员、2-黑名单)
            /// </summary>
            public int UserLevel { get; set; }

            /// <summary>
            /// 找回密码问题
            /// </summary>
            public string Question { get; set; }

            /// <summary>
            /// 答案
            /// </summary>
            public string Answer { get; set; }

            /// <summary>
            /// 注册终端(0-Web、1-安卓、2-IOS)
            /// </summary>
            public string Terminal { get; set; }

            /// <summary>
            /// 最后登录时间
            /// </summary>
            public DateTime LastTime { get; set; }


            /// <summary>
            /// 用户唯一标识
            /// </summary>
            public int identifier { get; set; }

        }

        public class ResponseDoctorClinicDTO
        {
            /// <summary>
            /// 义诊月份
            /// </summary>
            public string ClinicMonth { get; set; }

            /// <summary>
            /// 义诊日期列表
            /// </summary>
            public string ClinicDates { get; set; }

            /// <summary>
            /// 每天接受咨询次数
            /// </summary>
            public int AcceptCount { get; set; }

            /// <summary>
            /// 当前咨询次数
            /// </summary>
            public int CurrentCount { get; set; }

            /// <summary>
            /// 状态(0-关闭、1-开通)
            /// </summary>
            public bool State { get; set; }
        }
    }
}
