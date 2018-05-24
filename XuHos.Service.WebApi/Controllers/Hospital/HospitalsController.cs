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
    /// 医院
    /// </summary>
    [UserAuthenticate(IsValidUserType =false)]
    public class HospitalsController : ApiBaseController
    {
        BLL.HospitalService bll;

        public HospitalsController()
        {
        }

        /// <summary>
        /// 103 Base Data：103101
        /// 新增医院
        /// 前置条件：管理员登录
        /// </summary>
        /// <param name="requst">医院信息</param>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        [Route("~/Hospitals")]
        public ApiResult InsertEntity([FromBody]DTO.HospitalDto requst)
        {
            bll = new BLL.HospitalService(CurrentOperatorUserID, CurrentOperatorOrgID);
            var model = requst.Map<DTO.HospitalDto, Entity.Hospital>();
            if (bll.Insert(model))
            {
                return model.HospitalID.ToApiResultForObject();
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }

        /// <summary>
        /// 103 Base Data：103102
        /// 更新医院
        /// 前置条件：管理员登录
        /// </summary>
        /// <param name="requst">医院信息</param>
        /// <returns></returns>
        [HttpPut]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        [Route("~/Hospitals")]
        public ApiResult UpdateEneity([FromBody]DTO.HospitalDto requst)
        {
            bll = new BLL.HospitalService(CurrentOperatorUserID, CurrentOperatorOrgID);
            var model = requst.Map<DTO.HospitalDto, Entity.Hospital>();
            return bll.Update(model).ToApiResultForBoolean();
        }

        /// <summary>
        /// 103 Base Data：103103
        /// 删除医院
        /// 前置条件：管理员登录
        /// </summary>
        /// <param name="ID">医院编号</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("~/Hospitals")]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        public ApiResult DeleteEntity(string ID)
        {
            return bll.Delete(ID).ToApiResultForBoolean();
        }



        /**
         * @apiIgnore Not finished Method
           * @api {GET} /Hospitals/?ID=:ID 103104/获取医院详情
           * @apiGroup 103 Base Data
           * @apiVersion 4.0.0
           * @apiDescription 获取医院详情 
           * @apiPermission 所有人
           
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
                  
           * @apiParam {String} ID 医院编号 
           * @apiParamExample {json} 请求样例：
           *    ?ID=42FF1C61132E443F862510FF3BC3B03A
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {object} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           *                {"Data":{"HospitalID":"42FF1C61132E443F862510FF3BC3B03A","HospitalName":"康美医院","Intro":"<p>康美医院是由广东省卫生厅批准设立，中国制药百强企业、广东省百强民营企业、上海证券交易所上市公司康美药业股份有限公司（股票代码600518）投资创办的第一家大型非营利性综合医院。医院位于粤东商贸名城、全国著名中药材集散地、著名侨乡普宁市中心区，周围环境优美，交通便利，人口稠密，客商云集。医院按照三级甲等医院标准规划设计，占地面积100余亩，其中医疗区50多亩，医用建筑面积近12万平方米，一期可开放床位500张。</p>\n\n<p>医院功能完善，学科齐全，批准设立的一级学科有急诊科、内科、外科、妇产科、儿科、眼科、耳鼻喉科、口腔科、皮肤科、中医科、麻醉科、重症医学科、预防保健科等。内科系统计划开设的二级学科有呼吸内科、消化内科、心血管内科、神经内科、内分泌科、血液病科、肿瘤科等。外科系统计划开设的二级学科有普外科、骨科、神经外科、胸外科、泌尿外科、烧伤整形科、肛肠科等。开设的医技科室有影像科、检验科、超声科、功能科、病理科、药剂科、理疗科等。根据潮汕地区疾病谱和社会医疗需求，医院重点发展神经科、心血管科、肿瘤科、创伤外科、妇产科等优势学科。</p>\n\n<p>医院致力于打造粤东地区设备一流、技术一流、服务一流的地市级区域医疗中心，已经配置的医疗设备有西门子128层高配螺旋CT、1.5T高配核磁共振成像系统（MRI）、平板数字化多功能X线诊断系统、移动式数字化X线拍片系统、三维C臂X线成像系统（术中CT）、全数字化平板乳腺X线摄影系统、ACUSON-SC2000彩超、四维彩超，贝克曼高端全自动生化分析仪、全自动化学发光仪等大批高端设备。</p>\n\n<p>医院设有18间层流净化手术室、20张床位的重症监护中心（ICU）、装备32台透析机的血液透析中心，高标准产房和内镜中心等。</p>\n\n<p>医院立足当地，面向全国，引进大批优秀的专业技术人才，与北京、上海、广州、汕头等地著名医院和高等医学院校建立科研、教学、远程会诊、双向转诊等业务合作关系，聘请省内外著名专家、教授定期来院讲学、会诊、手术和技术指导。</p>\n\n<p>康美医院以服务社会、提高周边地区人民群众医疗保健水平为己任，秉承&ldquo;心怀苍生，大爱无疆&rdquo;核心价值观，借鉴国内外先进的医院管理模式，引进JCI国际认证体系，坚持以人才和技术为依托，以病人健康需求为中心，以医疗质量和医疗安全为核心的办院理念，注重内涵建设，优化服务流程，实施人性化服务，竭诚为普宁市和周边人民群众提供技术精湛、服务温馨、价格低廉、优质高效的医疗服务，倾情打造业界认可，政府信任，患者满意、诚信仁爱的康美医疗品牌。</p>\n\n<p>&nbsp;</p>","License":"YYZZ000001","LogoUrl":"/Uploads/hospital/201509/151815495260.png","Address":"广东省普宁市流沙新河西路38号","PostCode":"515300","Telephone":"(0663)2229222","Email":"km@kmlove.com.cn","ImageUrl":"/Uploads/hospital/201512/1.jpg","DepartmentCount":0,"DoctorCount":0,"Departments":[]},"Total":0,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 获取医院详情
        /// 前置条件：无

        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ID">预约编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Hospitals")]
        [IgnoreUserAuthenticate]
        public ApiResult GetEntity([FromUri]string ID)
        {
            bll = new BLL.HospitalService(CurrentOperatorUserID, CurrentOperatorOrgID);
            return bll.Single<HospitalDto>(ID).ToApiResultForObject();
        }

        /**
         * @apiIgnore Not finished Method
           * @api {GET} /Hospitals 103105/获取医院列表
           * @apiGroup 103 Base Data
           * @apiVersion 4.0.0
           * @apiDescription 通过关键字，分页获取医院列表 
           * @apiPermission 所有人
           
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
                  
           * @apiParam {int} CurrentPage=1 页码 
           * @apiParam {int} PageSize=10 分页大小
           * @apiParam {string} Keyword='' 关键字
           * @apiParamExample {json} 请求样例：
           *                   ?CurrentPage=1&PageSize=1&Keyword=
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {Array} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           *                   {"Data":[{"HospitalID":"ea3343853b2f479c88a826247a6a4677","HospitalName":"test2","Intro":"<p>\n\ttes2</p>","License":"test2","LogoUrl":"baiud.com","Address":"test2","PostCode":"422700","Telephone":"0755-5555555","Email":"tt@163.com","ImageUrl":"baiud.com","DepartmentCount":1,"DoctorCount":0},{"HospitalID":"FC79B50A79B241BFAC7F86C4DE3CEC74","HospitalName":"深圳市人民医院","Intro":"<p>深圳市人民医院，始建于1946年，前身是宝安县医院，1979年伴随深圳经济特区成立更名为深圳市人民医院。1994年被评为深圳首家&ldquo;三级甲等&rdquo;医院。1996年经国务院侨办批准成为暨南大学医学院第二附属医院，2005年升格为暨南大学第二临床医学院。伴随着经济特区的成长，深圳市人民医院已发展成为一个功能齐全、设备先进、人才结构合理、技术力量雄厚，集医疗、教学、科研、保健为一体的深圳市最大的现代化综合性医院。医院占地面积13.82万平方米，建筑面积21.3万平方米，编制病床2100张，开放病床2400张。2013年出院病人8.04万人次，门诊量300多万人次。目前医院有两个门诊部（一、二门诊部）、一个住院部（又称留医部）及一个分院（深圳市人民医院龙华分院）。</p>\r\n\r\n<p>医院现有呼吸内科、肾内科、消化内科、感染内科、内分泌科、胸外科、口腔科、麻醉科、妇科、产科、新生儿科、急诊科、病理科、检验科、临床护理及包含CT、放射、超声、介入、核医学在内的医学影像科16个省级重点学科，优势医学重点学科（群）7个、深圳市医学重点学科14个、深圳市医学重点实验室4个。2013年我院介入微创诊疗中心被评为亚洲冷冻治疗培训基地，承担起培训港澳台及中国南方地区专家的任务，凭着医院的整体医疗技术水平，吸引了香港、澳门以及深圳周边地区的大批患者，受到国内外媒体广泛关注。作为暨南大学第二临床医学院，拥有卫生部全科医生培训基地、暨南大学第二临床医学院博士后创新实践基地。医院现有内科学、外科学、妇产科学、儿科学等18个教研室。目前有博士生导师16名，硕士生导师107名。现已招收硕士生730余名、博士生40余名，本科生264余名。每年招收临床实习生近130余名，进修生 230余名。2013年全院申报包括国家自然科学基金在内的各级课题326项。在SCI收录期刊及核心期刊发表论文581篇。</p>\r\n\r\n<p>医院积极开展高层次、国际性的交流与合作，先后与德国纽伦堡大学医学院、不莱梅港中心医院、加拿大西安大略大学器官移植中心、美国休斯敦卫理公会医院、法国里尔大学医疗中心、韩国全南大学校病院、韩国大田宣医院建立起长期合作与交流平台，缔结友好医院，与国际医学大舞台进一步交融。2013年4月基于深圳市超级计算中心云平台的深圳市人民医院网络医院正式实运营，网络保健、远程监测、全程医疗等获得了社会的广泛好评，被列为深圳市健康产业重点扶持民生工程计划。深圳市人民医院以强烈的社会责任感积极承担着特区重大活动的医疗保障工作，用她的历史、文化、勇气、智慧，努力呵护着市民的健康，铸就深圳医疗的辉煌。</p>\r\n\r\n<p>&nbsp;</p>","License":"粤ICP备09113187号","LogoUrl":"/Uploads/hospital/201512/101453275417.png","Address":"深圳市东门北 路1017号","PostCode":"518020","Telephone":"0755-25533018","Email":"szhospital@163.com","ImageUrl":"/Uploads/hospital/201512/30.jpg","DepartmentCount":18,"DoctorCount":12}],"Total":10,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 获取医院列表
        /// 前置条件：无

        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Hospitals")]
        [IgnoreUserAuthenticate]
        [IgnoreAuthenticate]
        public ApiResult GetEntitys([FromUri]DTO.RequestHospitalQueryDTO request)
        {
            bll = new BLL.HospitalService(CurrentOperatorUserID, CurrentOperatorOrgID);
            if (request == null)
            {
                return bll.GetPageList().ToApiResultForList();
            }
            else
            {
                return bll.GetPageList(
                    request.CurrentPage,
                    request.PageSize,
                    request.Keyword).ToApiResultForList();
            }
        }
    }
}