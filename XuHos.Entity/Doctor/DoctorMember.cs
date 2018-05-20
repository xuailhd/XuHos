using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;
namespace XuHos.Entity
{    
    /// <summary>
    /// 医生的患者
    /// </summary>
    public partial class DoctorMember : AuditableEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DoctorMemberID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DoctorID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MemberID { get; set; }
      
        
    }
}
