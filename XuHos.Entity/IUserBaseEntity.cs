using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserBaseEntity : IAuditableEntity
    {
        string UserID
        { get; set; }
    }
}
