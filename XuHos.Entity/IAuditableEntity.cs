using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public interface IAuditableEntity
    {
        //Guid RowNumber { get; set; }
        string CreateUserID { get; set; }
        
        DateTime CreateTime { get; set; }

        string ModifyUserID { get; set; }

        DateTime? ModifyTime { get; set; }

        string DeleteUserID { get; set; }

        DateTime? DeleteTime { get; set; }

        bool IsDeleted { get; set; }

    }
}
