using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;

namespace XuHos.BLL.Common.DTOs
{
    public class RequestUserOperateLogDTO<T>
    {
        public string UserID { get; set; }

        public EnumUserType UserType { get; set; }

        public EnumUserOperationType OperationType { get; set; }

        public DateTime OpTime { get; set; }

        public T OperationData { get; set; }

        public string Remark { get; set; }
    }
}