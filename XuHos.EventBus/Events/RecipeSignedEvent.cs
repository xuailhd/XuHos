using System;
using System.Collections.Generic;
using System.Linq;
using XuHos.Common.Enum;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 处方签名完成。DB已更新State
    /// </summary>
    public class RecipeSignedEvent : BaseEvent, IEvent
    {
        public string RecipeFileId { get; set; }

        public string OPDRegisterID { get; set; }

        public string OrgnazitionID { get; set; }
        public bool IsDrugstoreRecipe { get; set; }
        public string RecipeNo { get; set; }
    }
}