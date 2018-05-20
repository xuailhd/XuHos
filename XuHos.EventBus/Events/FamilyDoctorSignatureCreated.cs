using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 家庭医生签约创建后
    /// </summary>
    public class FamilyDoctorSignatureCreated : BaseEvent, IEvent
    {
        /// <summary>
        /// 签约编号
        /// </summary>
        public string SignatureID { get; set; }

        public string DoctorID { get; set; }
        public bool SignatureStatusIsSigned { get; set; }

        public EventFDSignatureDto FDSignature { get; set; }

        public EventDoctorGroupDto DoctorGroup { get; set; }
    }

    public class EventFDSignatureDto
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgnazitionName { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string SignatureID { get; set; }
        /// <summary>
        /// 医生团队所属机构ID（甲方）
        /// </summary>
        public string OrgnazitionID { get; set; }

        /// <summary>
        /// 签名用户ID(乙方)
        /// </summary>
        public string SignatureUserID { get; set; }

        /// <summary>
        /// 签名用户姓名(乙方)
        /// </summary>
        public string SignatureUserName { get; set; }

        public Common.Enum.EnumUserGender SignatureUserGender { get; set; }

        public string FamilyFN { get; set; }

        /// <summary>
        /// 签名用户身份证(乙方)
        /// </summary>
        public string SignatureUserIDNumber { get; set; }

        /// <summary>
        /// 手机号(乙方)
        /// </summary>
        public string Mobile { get; set; }

        public string CityName { get; set; }
        public string DistrictName { get; set; }

        public string SubdistrictName { get; set; }

        public string Address { get; set; }


        public List<EventFDSignatureMemberDto> Members { get; set; }
    }

    public class EventDoctorGroupDto
    {
        public string LeaderName { get; set; }

        public string GroupName { get; set; }

        public int MemberCount { get; set; }

        public List<string> UserIDs { get; set; }
    }

    public class EventFDSignatureMemberDto
    {
        public Common.Enum.EnumUserRelation Relation { get; set; }

        public string MemberName { get; set; }

        public Common.Enum.EnumUserGender Gender { get; set; }

        public string IDNumber { get; set; }
    }
}
