using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XuHos.Entity;
using XuHos.Extensions;

using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Service.Infrastructure.Filters;
using XuHos.Common.Enum;

namespace XuHos.WebApi.Controllers
{

    /// <summary>
    /// 科室
    /// </summary>
    public class DepartmentsController : ApiBaseController
    {
        BLL.HospitalDepartmentService bll;

        public DepartmentsController()
        {
        }

        /// <summary>
        /// 103 Base Data：103001
        /// 新增科室
        /// 前置条件：管理员登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="requst">科室信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/Departments")]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        public ApiResult InsertEntity([FromBody]DTO.HospitalDepartmentDTO requst)
        {
            var model = requst.Map<DTO.HospitalDepartmentDTO, Entity.HospitalDepartment>();
            bll = new BLL.HospitalDepartmentService(CurrentOperatorUserID);
            if (bll.Insert(model))
            {
                return model.ToApiResultForObject();
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }

        /// <summary>
        /// 103 Base Data：103002
        /// 更新科室
        /// 前置条件：管理员登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="requst">科室信息</param>
        /// <returns></returns>
        [HttpPut]
        [Route("~/Departments")]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        public ApiResult UpdateEntity([FromBody]DTO.HospitalDepartmentDTO requst)
        {
            bll = new BLL.HospitalDepartmentService(CurrentOperatorUserID);
            var model = requst.Map<DTO.HospitalDepartmentDTO, Entity.HospitalDepartment>();
            return bll.Update(model).ToApiResultForBoolean();
        }

        /// <summary>
        /// 103 Base Data：103003 
        /// 删除科室
        /// 前置条件：管理员登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ID">科室编号</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("~/Departments")]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        public ApiResult DeleteEntity(string ID)
        {
            bll = new BLL.HospitalDepartmentService(CurrentOperatorUserID);
            return bll.Delete(ID).ToApiResultForBoolean();
        }

        /**
         * @apiIgnore Not finished Method
          * @api {GET} /Departments/?ID=:ID 103004/获取科室详情
          * @apiGroup 103 Base Data
          * @apiVersion 4.0.0
          * @apiDescription 获取科室详情 
          * @apiPermission 所有人
          
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
                 
          * @apiParam {String} ID 科室编号 
          * @apiParamExample {json} 请求样例：
          *                   ?ID=068EC7030C0D40DD9EE436601C7F34FA
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {object} Data 业务数据
          * @apiSuccessExample {json} 返回样例:
          *{"Data":{"DepartmentID":"068EC7030C0D40DD9EE436601C7F34FA","HospitalID":"50A8B2CE955644CF8750200244B61D14","DepartmentName":"神经内科","Intro":"<p>神经内科成立于1999年底，当时仅有3 张床位，医生4 名，并开始开展TCD、电生理等检查。随着医院的发展，我科规模逐渐扩大，2001年9月19日神经内科成为一个独立病区，设置床位47张，张海鸥教授担任主任，招收中山大学、同济医大、湘雅医学院等毕业生为在编人员。目前开放病房床位48张，开设5个专家门诊诊室，配设脑功能康复室和神经电生理室。现有主任医师5人，副主任医师8人，研究生导师4人，博士5人，硕士7人。诊疗范围包括头痛、眩晕、癫痫、睡眠障碍、脊髓病变、中枢神经系统脱髓鞘疾病、中枢神经系统感染、帕金森病以及各类脑血管病的诊断与治疗。技术优势如下：1.对神经系统疑难、危重病人的诊治积累了较丰富的临床经验；2.开展脑血管病的超早期溶栓治疗；3.可对各类瘫痪病人实施超早期康复治疗；4.开展经颅超声多普勒、脑血管彩色超声、常规脑电图、24h动态脑电图、诱发电位以及肌电图检测；5.开展睡眠呼吸障碍的监测与治疗；6、坚持每周三全科大查房、会诊，专门探讨、解决疑难危重病人的诊断和治疗。目前日均门诊量300余人次，每月平均出院150人次。2009年病房出院1800余人次。<br />\n在教学方面，我们已经成为北京大学神经病学硕士点、汕头大学神经病学硕士点，长期承担北京大学医学部、广东医学院、汕头大学医学院、安徽医科大学等统招硕士生、七年制硕士生、五年制本科生、五年制护理本科、三年制专升本等多层次教学。硕士导师4名，现每年招收硕士生2-3人，进修医师2-3人。<br />\n在科研方面，已承担省、市科技计划项目10余项，在各类学术期刊上发表论文百余篇。<br />\n2013年4月本学科被评为院级重点学科。<br />\n&nbsp;</p>\n","Doctors":[]},"Total":0,"Status":0,"Msg":""}
      **/
        /// <summary>
        /// 获取科室详情
        /// 前置条件：无

        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ID">科室编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Departments")]
        [IgnoreUserAuthenticate]
        public ApiResult GetEntity([FromUri]string ID)
        {
            bll = new BLL.HospitalDepartmentService(CurrentOperatorUserID);
            return bll.Single<HospitalDepartmentDTO>(ID).ToApiResultForObject();
        }


        /**
         * @apiIgnore Not finished Method
           * @api {GET} /Departments 103005/获取科室列表
           * @apiGroup 103 Base Data
           * @apiVersion 4.0.0
           * @apiDescription 通过关键字，分页获取医院列表 
           * @apiPermission 所有人
           
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
                  
           * @apiParam {int} CurrentPage=1 页码 
           * @apiParam {int} PageSize=10 分页大小
           * @apiParamExample {json} 请求样例：
           *                   ?CurrentPage=1&PageSize=1
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {Array} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           *{"Data":[{"DepartmentID":"709b9f481f7f4a25aaa20da251afcdb8","HospitalID":"ea3343853b2f479c88a826247a6a4677","DepartmentName":"test","Intro":"<p>\n\ttst</p>","Doctors":[]},{"DepartmentID":"0B9B95003A72405690F18019C32EB0CB","HospitalID":"FC79B50A79B241BFAC7F86C4DE3CEC74","DepartmentName":"心血管内科","Intro":"<p>心内科：冠心病、心绞痛、ACS、心源性休克、恶性心律失常、高血压、结构性心脏病、心律失常、射频消融术、心脏起搏植入术、主动脉夹层、主动脉瘤、急性主动脉综合征、CRT-D、ICD植入、心力衰竭、心肌病、各种心血管疾病的心脏康复等。</p>\n\n<p>深圳市人民医院心脏中心/心血管内科成立于1995年，是深圳市医学重点专科，是广东省住院医师培训基地、广东省专科医师培训基地、广东省全科医师培训基地；是广东省起搏与电生理学会副主任委员单位、深圳市医学会心血管分会主任委员单位；是卫生部国家药物临床基地、深圳市健康教育研究所及深圳市高血压健康教育基地、暨南大学博士、硕士培养基地，是美国ACC（美国心脏病学院）在深圳市唯一一家培训基地；是集临床、教学、科研、预防于一体，深圳市心血管疾病介入治疗的领头科室，在华南乃至全国享有广泛声誉。</p>\n\n<p>深圳市人民医院心脏中心/心血管内科，是深圳地区规模最大、技术力量最雄厚、融无创与有创协同治疗于一体的心血管疾病诊治中心，科室硬件设施一流。现设有床位102张，包括心内科一区、心内科二区、心脏重症监护室（CCU）三个病区，拥有1个独立导管室，设有心内科专科门诊、心血管疾病康复室、心血管无创检查室、心脏彩超室、心电图室、活动平板室、心电生理室等部门。</p>\n\n<p>深圳市人民医院心血管内科是我院的重点学科，创建于1998年，护理人员由当年的12人发展至今天的48人。其中本科学历30人，大专学历18人；中级职称9人，初级职称39人。心内科多年来承担了大量的临床护理工作和教学任务，是一支技术精湛、服务一流的优秀护理团队。在心血管危急重症的抢救配合、心导管技术的护理等方面积累了丰富的经验，每年配合医生完成心脏介入手术近千例，为患者提供了安全、高效、优质的护理服务。</p>\n","Doctors":[]}],"Total":53,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 查询科室信息
        /// 前置条件：无

        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Departments")]
        [IgnoreUserAuthenticate]
        public ApiResult GetEntitys([FromUri]DTO.RequestHospitalQueryDTO request)
        {
            bll = new BLL.HospitalDepartmentService(CurrentOperatorUserID);
            if (request == null)
            {
                return bll.GetPageList<DTO.HospitalDepartmentDTO>(
                                  1, int.MaxValue).ToApiResultForList();
            }
            else
            {
                return bll.GetPageList<DTO.HospitalDepartmentDTO>(
                    request.CurrentPage,
                    request.PageSize).ToApiResultForList();
            }
        }

        /**
         * @apiIgnore Not finished Method
           * @api {Get} /Departments/Options  103009获取科室选项
           * @apiGroup 103 Base Data
           * @apiVersion 4.0.0
           * @apiDescription 获取科室选项
           * @apiPermission 所有人
           
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写
           * @apiParamExample {json} 请求样例：
           * /Departments/Options?HospitalID=XXX
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {Array} Data 数据
           * @apiSuccessExample {json} 返回样例: 
           {
                "Data": [
                    {
                        "Value": "科室ID",
                        "Text": "科室名称",
                        "Data": {
                            "CAT_NO": "公共科室编码"
                        }
                    }
                ],
                "Total": 10,
                "Status": 0,
                "Msg": ""
            }
       **/
        /// <summary>
        /// 获取科室选项
        /// </summary>
        /// <param name="HospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Departments/Options")]
        [IgnoreUserAuthenticate]
        [IgnoreAuthenticate]
        //[ApiOutputCacheFilter(60 * 60 * 2, 60 * 60 * 1, false)]
        public object GetDepartmentDDL(string HospitalID)
        {
            var list = new BLL.HospitalDepartmentService(XuHos.Service.Infrastructure.SecurityHelper.LoginUser.UserID).GetDepartmentDropdownList(HospitalID);
            return list.ToApiResultForObject();
        }
    }
}