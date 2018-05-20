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

namespace XuHos.WebApi.Controllers
{

    /// <summary>
    /// 系统字典
    /// </summary>
    public class SysDictsController : ApiBaseController
    {
        BLL.Common.CommonBaseService<SysDict> bll;

        public SysDictsController()
        {
        }

        /**
        * @api {GET} /SysDicts 103602/获取系统字典
        * @apiGroup 103 Base Data
        * @apiVersion 4.0.0
        * @apiDescription 获取系统字典 
        * @apiPermission 所有人
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录数
        * @apiSuccess (Response) {Array} Data 字典信息
        * @apiSuccessExample {json} 返回样例:
        *{"Data":[{"DicID":"0AF51F692F1248BFA347FB6189798882","DictTypeID":"UserLevel","DicCode":"2","CNName":"黑名单","ENName":"Blacklist","OrderNo":2,"Remark":"用户级别"},{"DicID":"0E666B51A06C4C0E92D63A72C7AAB30B","DictTypeID":"Terminal","DicCode":"0","CNName":"网站","ENName":"Web","OrderNo":0,"Remark":"注册终端"},{"DicID":"1BEDB6F1B0854B958942ADAA50DEF63A","DictTypeID":"UserState","DicCode":"1","CNName":"冻结","ENName":"Freeze","OrderNo":2,"Remark":"用户状态"},{"DicID":"40C36DB0FA584AD28A7C3180CB6ABDED","DictTypeID":"UserType","DicCode":"1","CNName":"普通用户","ENName":"User","OrderNo":2,"Remark":"用户类型"},{"DicID":"5762EC9EA202408A9924BE61E33621EF","DictTypeID":"UserType","DicCode":"0","CNName":"系统用户","ENName":"Admin","OrderNo":1,"Remark":"用户类型"},{"DicID":"92FCF9C03E134F28B3EE05ADAEC45D54","DictTypeID":"UserType","DicCode":"2","CNName":"医生用户","ENName":"Doctor","OrderNo":3,"Remark":"用户类型"},{"DicID":"A9A50AFCB3A2410794A3948FCA9807F0","DictTypeID":"UserState","DicCode":"0","CNName":"正常","ENName":"Normal","OrderNo":1,"Remark":"用户状态"},{"DicID":"ACCD1DA9CD554AAF962A6C238CB0D608","DictTypeID":"Terminal","DicCode":"2","CNName":"IOS","ENName":"IOS","OrderNo":2,"Remark":"注册终端"},{"DicID":"C1AFDBAAED89495494F5E7952AF7E2C3","DictTypeID":"UserLevel","DicCode":"0","CNName":"普通用户","ENName":"User","OrderNo":0,"Remark":"用户级别"},{"DicID":"D7F03C8B1F0945BFA4D7F3AD0F821C18","DictTypeID":"Terminal","DicCode":"1","CNName":"安卓","ENName":"Andriod","OrderNo":1,"Remark":"注册终端"},{"DicID":"E3ADA028A6B146B1A55901595857819F","DictTypeID":"UserState","DicCode":"2","CNName":"销户","ENName":"Cancellation","OrderNo":3,"Remark":"用户状态"},{"DicID":"E6720C28F454445EB42D6718DE4966BB","DictTypeID":"UserType","DicCode":"3","CNName":"医院用户","ENName":"Hospital","OrderNo":4,"Remark":"用户类型"},{"DicID":"F23199BF86D64FF48C80026EA413FE6B","DictTypeID":"UserLevel","DicCode":"1","CNName":"会员","ENName":"VIP","OrderNo":1,"Remark":"用户级别"}],"Total":13,"Status":0,"Msg":""}
        **/
        /// <summary>
        /// 获取系统字典
        /// 前置条件：无
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/SysDicts")]
        [IgnoreUserAuthenticate]
        [ApiOperateNotTrack]
        [ApiOutputCacheFilterAttribute(60 * 60 * 2, 60 * 60*1, false)]
        public ApiResult GetEntitys()
        {
            bll = new BLL.Common.CommonBaseService<SysDict>(CurrentOperatorUserID);
            return bll.GetPageList<SysDictDTO>().ToApiResultForList();
        }

    }
}