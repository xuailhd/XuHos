using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public class AuditableEntity: IAuditableEntity
    {

        public AuditableEntity()
        {
            //RowNumber = Guid.NewGuid();
            CreateUserID = string.Empty;
            CreateTime = DateTime.Now;
            ModifyUserID = string.Empty;
            ModifyTime = DateTime.Now;
            //DeleteUserID = string.Empty;
            //DeleteTime = DateTime.Now;
            IsDeleted = false;
        }

        /// <summary>
        /// 创建用户ID
        /// </summary>
        [Column(TypeName ="varchar")]
        [MaxLength(32)]
        public string CreateUserID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改用户ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ModifyUserID { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 删除用户ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DeleteUserID { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column(TypeName ="bit")]
        public bool IsDeleted { get; set; }

    }
}
