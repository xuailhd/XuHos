using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;

namespace XuHos.Service.Infrastructure
{
    public class BundleConfig
    {
        public static void RefreshApplicationCache()
        {
            #region 启动刷新系统配置
            try
            {

                SysConfigService.Get<Common.Config.Sections.Api>(false);
                SysConfigService.Get<Common.Config.Sections.IMGStore>(false);
                SysConfigService.Get<Common.Config.Sections.IM>(false);
                SysConfigService.Get<Common.Config.Sections.Agora>(false);
                SysConfigService.Get<Common.Config.Sections.Pay>(false);
                SysConfigService.Get<Common.Config.Sections.Mongodb>(false);


                #region 清除缓存
                new SysAccessAccountService().ClearAccountCache();
                #endregion
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
            }

            #endregion
        }

        public static void RegisterApplicationConfig()
        {
            //序列号生成器
            XuHos.Common.Utility.SeqIDHelper.RegisterSeqWorker(0, 0);

            #region 设置Redius缓存服务器设置

            //获取Redis缓存配置,首次不从缓存中获取
            var redisConfig = SysConfigService.Get<Common.Config.Sections.Redis>(false);
            //注册Redis处理程序，并指定默认Db
            XuHos.Common.Cache.Manager.Register((dbNum) => 
            XuHos.Common.Cache.Redis.StackExchangeImplement.RedisCacheManage.Create(redisConfig, dbNum),
            0);
            #endregion

            #region 注册支付相关配置
            //获取Redis缓存配置,首次不从缓存中获取
            var payConfig = SysConfigService.Get<Common.Config.Sections.Pay>();
            XuHos.Common.Pay.Configuration.Register(payConfig);

            var wxPayAppIdList = payConfig.WXPaySellerIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var kmPayAppIdList = payConfig.KMPaySellerIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var aliPayAppIdList = payConfig.AliPaySellerIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var appId in wxPayAppIdList)
            {
                XuHos.Common.Pay.Configuration.Register(SysConfigService.Get<Common.Config.Sections.Pay.WXPay>(true, (string)("[" + appId + "]")), (string)appId);
            }

            foreach (var appId in kmPayAppIdList)
            {
                XuHos.Common.Pay.Configuration.Register(SysConfigService.Get<Common.Config.Sections.Pay.KMPay>(true, (string)("[" + appId + "]")), (string)appId);
            }

            foreach (var appId in aliPayAppIdList)
            {
                XuHos.Common.Pay.Configuration.Register(SysConfigService.Get<Common.Config.Sections.Pay.AliPay>(true, (string)("[" + appId + "]")), (string)appId);
            }
            #endregion

            #region 注册图片配置
            var imgConfig = SysConfigService.Get<XuHos.Common.Config.Sections.IMGStore>();
            XuHos.DTO.Common.ImageBaseDTO.UrlPrefix = imgConfig.UrlPrefix;
            #endregion

            #region 注册消息队列配置
            var mqConfig = SysConfigService.Get<XuHos.Common.Config.Sections.MQ>();
            XuHos.EventBus.Configuration.RegisterConfig(mqConfig.HostName, int.Parse(mqConfig.Port), mqConfig.UserName, mqConfig.Password, mqConfig.VirtualHost);

            #endregion

            #region 注册 Mongodb 配置
            var mongodbConfig = SysConfigService.Get<Common.Config.Sections.Mongodb>();
            XuHos.DAL.Mongodb.MongoDbHelper.RegisterConfig(mongodbConfig.ConnectionString, mongodbConfig.CollectionName);
            #endregion

            #region 注册云通信和配置配置
            var imConfig = SysConfigService.Get<Common.Config.Sections.IM>();
            //var agoraConfig = SysConfigService.Get<Common.Config.Sections.Agora>();
            XuHos.Integration.QQCloudy.Configuration.RegisterConfig(imConfig);
            XuHos.Integration.WechatApp.Configuration.RegisterConfig(imConfig);
            #endregion

            #region 注册Api出站入站日志(使用Mongodb记录日志)
            XuHos.Common.Log.Configuration.Register(new XuHos.DAL.Mongodb.MongodbApiTrackLogAppender());
            #endregion

            //XuHos.Integration.ShortMessage.SMSHelper.RegisterYunZX(SysConfigService.Get<Common.Config.Sections.SMSYunZX>());

            //XuHos.Integration.ShortMessage.SMSHelper.RegisterYunPian(SysConfigService.Get<Common.Config.Sections.SMSYunPian>());
        }

    }
}
