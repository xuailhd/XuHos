using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{
    public class Api : IConfigSection
    {
        public string AppSecret
        { get; set; }

        public string AppId
        { get; set; }

        public string AppKey
        { get; set; }

        public string WebApiUrl { get; set; }
     
        /// <summary>
        /// 等同于 医院ID， 对接会员系统
        /// </summary>
        public string OrgID { get; set; }

        /// <summary>
        /// 用户端接口地址
        /// </summary>
        public string UserApiUrl
        { get; set; }

        /// <summary>
        /// 管理端接口地址
        /// </summary>
        public string AdminApiUrl
        { get; set; }


        /// <summary>
        /// 医生端接口地址
        /// </summary>
        public string DoctorApiUrl
        { get; set; }

        /// <summary>
        /// 远程会诊接口地址
        /// </summary>
        public string ConsultationApiUrl
        { get; set; }

        /// <summary>
        /// 公共接口地址
        /// </summary>
        public string CommonApiUrl
        { get; set; }

        /// <summary>
        /// 药店端接口地址
        /// </summary>
        public string DrugStoreApiUrl
        { get; set; }

        /// <summary>
        /// 健康服务站接口地址
        /// </summary>
        public string HealthstationApiUrl
        { get; set; }

        /// <summary>
        /// 家庭医生平台接口
        /// </summary>
        public string FamilyDoctorPlatformApiUrl
        { get; set; }

        /// <summary>
        /// 视频录制接口地址
        /// </summary>
        public string ApiGatewayUrl
        { get; set; }
    }
}
