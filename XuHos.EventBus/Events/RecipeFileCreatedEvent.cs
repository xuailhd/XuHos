using System;
using System.Collections.Generic;
using System.Linq;
using XuHos.Common.Enum;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 处方文件已生成事件，已生成PDF与封面图
    /// </summary>
    public class RecipeFileCreatedEvent : BaseEvent, IEvent
    {
        public string RecipeFileId { get; set; }

        public string OPDRegisterID { get; set; }

        public string OrgnazitionID { get; set; }
        public bool IsDrugstoreRecipe { get; set; }
        public string RecipeNo { get; set; }
    }
}