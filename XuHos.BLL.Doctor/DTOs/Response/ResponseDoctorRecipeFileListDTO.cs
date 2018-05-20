using XuHos.Common.Enum;
using XuHos.DTO;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    /// <summary>
    /// 医生处方
    /// </summary>
    public class ResponseDoctorRecipeFileListDTO : ImageBaseDTO
    {
        /// <summary>
        /// 处方ID
        /// </summary>
        public string RecipeFileID { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 患者性别（0-男、1-女、2-未知）
        /// </summary>
        /// <summary>
        public EnumUserGender MemberGender { get; set; }

        /// <summary>
        /// 患者出生日期
        /// </summary>
        public string MemberBirthday { get; set; }

        /// <summary>
        /// 处方名称
        /// </summary>
        public string RecipeName { get; set; }

        /// <summary>
        /// 处方时间
        /// </summary>
        public string RecipeDate { get; set; }

        /// <summary>
        /// 预约登记ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 处方签名业务ID
        /// </summary>
        public string SignatureID { get; set; }

        int _Age;
        public int MemberAge
        {
            get
            {
                if (_Age > 0)
                {
                    return _Age;
                }
                else
                {
                    if (!string.IsNullOrEmpty(MemberBirthday))
                    {
                        DateTime dt;

                        if (DateTime.TryParse(MemberBirthday, out dt) == true)
                        {
                            _Age = DateTime.Now.Year - dt.Year;
                            if (DateTime.Now.Month >= dt.Month && DateTime.Now.Day >= dt.Day)
                            {
                                _Age += 1;
                            }
                            return _Age;
                        }
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            set
            {
                _Age = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public EnumRecipeFileStatus RecipeFileStatus { get; set; }

        /// <summary>
        /// 诊断信息
        /// </summary>
        public List<string> Diagnoses { get; set; }

    }

}
