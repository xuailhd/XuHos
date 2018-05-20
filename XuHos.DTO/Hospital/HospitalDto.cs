using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponseHospitalBaseDTO
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        
        public string LogoUrl { get; set; }

        public string Mp4Url { get; set; }
        public string ImageUrl { get; set; }
        public string Mp4PreviewUrl { get; set; }
        public string ListImageUrl { get; set; }
        /// <summary>
        /// 首页主题样式
        /// </summary>
        public string HomePageTheme { get; set; }

        public string LogoFullUrl
        {
            get
            {
                return Common.ImageBaseDTO.PaddingUrlPrefix(LogoUrl);
            }
        }
        public string Mp4FullUrl
        {
            get
            {
                return Common.ImageBaseDTO.PaddingUrlPrefix(Mp4Url);
            }
        }
        public string ImageFullUrl
        {
            get
            {
                return Common.ImageBaseDTO.PaddingUrlPrefix(ImageUrl);
            }
        }
        public string Mp4PreviewFullUrl
        {
            get
            {
                return Common.ImageBaseDTO.PaddingUrlPrefix(Mp4PreviewUrl);
            }
        }
        public string ListImageFullUrl
        {
            get
            {
                return Common.ImageBaseDTO.PaddingUrlPrefix(ListImageUrl);
            }
        }
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
        string _ListImageUrl;

        public string ListImageUrl
        {
            set
            {
                _ListImageUrl = value;
            }
            get
            {
                return PaddingUrlPrefix(_ListImageUrl);
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

        string _ImageUrl;

        /// <summary>
        /// 图片
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

        public XuHos.Common.Enum.EnumOrgType OrgType { set; get; }

        /// <summary>
        /// 科室总数
        /// </summary>
        public int DepartmentCount { get; set; }

        /// <summary>
        /// 医生总数
        /// </summary>
        public int DoctorCount { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 首页主题样式
        /// </summary>
        public string HomePageTheme { get; set; }
        /// <summary>
        /// 医院科室
        /// </summary>
        public List<HospitalDepartmentDTO> Departments { get; set; }

        /// <summary>
        /// 医生
        /// </summary>
        public List<DoctorDto> Doctors { get; set; }

        /// <summary>
        /// 外接系统数据关联
        /// </summary>
        public ConversationExternalDTO ConversationExternals { get; set; }


        /// <summary>
        /// 医生信息
        /// </summary>
        public class DoctorDto
        {
            /// <summary>
            /// 医生ID
            /// </summary>
            public string DoctorID { get; set; }

            /// <summary>
            /// 医生ID
            /// </summary>
            public string DoctorName { get; set; }

            /// <summary>
            /// 用户ID
            /// </summary>
            public string UserID { get; set; }

            /// <summary>
            /// 性别（0-男、1-女、2-未知）
            /// </summary>
            public int Gender { get; set; }

            /// <summary>
            /// 婚姻情况(0-未婚、1-已婚、2-未知)
            /// </summary>
            public int Marriage { get; set; }

            /// <summary>
            /// 出生日期
            /// </summary>
            public string Birthday { get; set; }

            /// <summary>
            /// 证件类型
            /// </summary>
            public int IDType { get; set; }

            /// <summary>
            /// 证件号码
            /// </summary>
            public string IDNumber { get; set; }

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
            /// 特长
            /// </summary>
            public string Specialty { get; set; }

            /// <summary>
            /// 区域编码
            /// </summary>
            public string areaCode { get; set; }


            /// <summary>
            /// 医院等级
            /// </summary>
            public string Grade { get; set; }



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
            /// 科室ID
            /// </summary>
            public string DepartmentID { get; set; }

            public string DepartmentName { get; set; }

            public UserDTO User { get; set; }

        }

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
    }


    public class DruginfoDTO: ImageBaseDTO
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
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 店长姓名
        /// </summary>
        public string DrugstoreManageName { get; set; }

        /// <summary>
        /// 店长/门店电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }

        string _LogoUrl;

        /// <summary>
        /// 图片
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
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
