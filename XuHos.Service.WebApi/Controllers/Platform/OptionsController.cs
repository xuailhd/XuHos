using XuHos.BLL;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Enum;
using XuHos.DTO.Common;
using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace XuHos.WebApi.Controllers.Common
{
    public class OptionsController: ApiController
    {

        /**
      * @api {GET} /options 103601/获取系统选项
      * @apiGroup 103 Base Data
      * @apiVersion 4.0.0
      * @apiDescription 获取系统选项 
      * @apiPermission 所有人
      * @apiHeader {String} apptoken Users unique access-key.
      * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
      * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
      * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
      * @apiParam {string} optionName 选项编码
      * @apiSuccess (Response) {String} Msg 提示信息 
      * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
      * @apiSuccess (Response) {int} Total 总记录数
      * @apiSuccess (Response) {Array} Data 配置信息
      * @apiSuccessExample {json} 返回样例（EMR）:
      *{{"Data":[{"Key":"住院病案首页","Value":"住院病案首页"},{"Key":"入院记录","Value":"入院记录"},
      * {"Key":"出院记录","Value":"出院记录"},{"Key":"麻醉记录","Value":"麻醉记录"},
      * {"Key":"CysC+肝全+肾全+脂全","Value":"CysC+肝全+肾全+脂全"},{"Key":"血常规","Value":"血常规"},
      * {"Key":"尿常规","Value":"尿常规"},{"Key":"凝血1+D-Dimer","Value":"凝血1+D-Dimer"},{"Key":"ESR","Value":"ESR"},
      * {"Key":"血型(ABO+RhD)","Value":"血型(ABO+RhD)"},{"Key":"输血8项","Value":"输血8项"},{"Key":"Cyfra21-1+TPS","Value":"Cyfra21-1+TPS"},
      * {"Key":"肝功2+肾功","Value":"肝功2+肾功"},{"Key":"CysC","Value":"CysC"},{"Key":"肝全+肾全+脂全","Value":"肝全+肾全+脂全"},
      * {"Key":"甲功2+甲功3","Value":"甲功2+甲功3"},{"Key":"C13","Value":"C13"},{"Key":"肺功能报告单","Value":"肺功能报告单"},{"Key":"核医学检查报告单","Value":"核医学检查报告单"},{"Key":"医嘱单","Value":"医嘱单"},{"Key":"影像诊断报告单","Value":"影像诊断报告单"},{"Key":"超声诊断报告","Value":"超声诊断报告"},{"Key":"超声心动图诊断报告","Value":"超声心动图诊断报告"},{"Key":"CT影像诊断报告单","Value":"CT影像诊断报告单"},{"Key":"腹腔镜肾脏手术记录","Value":"腹腔镜肾脏手术记录"},{"Key":"X线影像诊断报告报告单","Value":"X线影像诊断报告报告单"},{"Key":"彩色病历检查报告","Value":"彩色病历检查报告"}],
      * "Total":0,"Status":0,"Msg":"操作成功"}}             
      **/
        /// <summary>
        /// 获取系统选项
        /// 前置条件：无
        /// </summary>
        /// <param name="optionName">选项名称（gender/idtype/relationship/disease/position/unit/chargetype/controllertype）</param>
        /// <returns></returns>
        [Obsolete("移至CommonApi的Config/GetOptionList")]
        [IgnoreAuthenticate]
        [HttpGet]
        [Route("~/options")]
        public IHttpActionResult GetOptionlist(string optionName)
        {
            var response = new ApiResult();
            response.Data = null;
            switch (optionName.Trim().ToLower())
            {
                case "gender":
                    {
                        response.Data = CodeService.GetGenders().ToList();
                    }; break;
                case "idtype":
                    {
                        response.Data = CodeService.GetIDTypes().ToList();
                    }; break;
                case "relationship":
                    {
                        response.Data = CodeService.GetRelationships().ToList();
                    }; break;
                case "disease":
                    {
                        response.Data = CodeService.GetDiseases().ToList();
                    }; break;
                case "position":
                    {
                        response.Data = CodeService.GetDoctorTitles().ToList();
                    }; break;
                case "unit":
                    {
                        response.Data = CodeService.GetUnits().ToList();
                    }; break;
                case "chargetype":
                    {
                        response.Data = CodeService.GetChargeTypes().ToList();
                    }; break;
                case "controllertype":
                    {
                        response.Data = CodeService.GetControllerTypes().ToList();
                    }; break;
                case "emrname":
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("住院病案首页", "住院病案首页");
                        dic.Add("入院记录", "入院记录");
                        dic.Add("出院记录", "出院记录");
                        dic.Add("麻醉记录", "麻醉记录");
                        dic.Add("CysC+肝全+肾全+脂全", "CysC+肝全+肾全+脂全");
                        dic.Add("血常规", "血常规");
                        dic.Add("尿常规", "尿常规");
                        dic.Add("凝血1+D-Dimer", "凝血1+D-Dimer");
                        dic.Add("ESR", "ESR");
                        dic.Add("血型(ABO+RhD)", "血型(ABO+RhD)");
                        dic.Add("输血8项", "输血8项");
                        dic.Add("Cyfra21-1+TPS", "Cyfra21-1+TPS");
                        dic.Add("肝功2+肾功", "肝功2+肾功");
                        dic.Add("CysC", "CysC");
                        dic.Add("肝全+肾全+脂全", "肝全+肾全+脂全");
                        dic.Add("甲功2+甲功3", "甲功2+甲功3");
                        dic.Add("C13", "C13");
                        dic.Add("肺功能报告单", "肺功能报告单");
                        dic.Add("核医学检查报告单", "核医学检查报告单");
                        dic.Add("医嘱单", "医嘱单");
                        dic.Add("影像诊断报告单", "影像诊断报告单");
                        dic.Add("超声诊断报告", "超声诊断报告");
                        dic.Add("超声心动图诊断报告", "超声心动图诊断报告");
                        dic.Add("CT影像诊断报告单", "CT影像诊断报告单");
                        dic.Add("腹腔镜肾脏手术记录", "腹腔镜肾脏手术记录");
                        dic.Add("X线影像诊断报告报告单", "X线影像诊断报告报告单");
                        dic.Add("彩色病历检查报告", "彩色病历检查报告");
                        response.Data = dic.ToList();
                    }; break;
            }
            if (response.Data == null)
            {
                return NotFound();
            }
            //var codes = from o in ret select new Option { Key = o.Key, Value = o.Value };
            return Ok(response);
        }
    }
}