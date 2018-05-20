using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;
using XuHos.Entity;
using System.Data;
using XuHos.DAL.EF.DbCommandInterceptors;
using EntityFramework.Extensions;
namespace XuHos.DAL.EF
{

    public class DBEntities : DbContext
    {
        public DBEntities()
          : base("name=DBEntities")
        {
            //Database.SetInitializer<DBEntities>(new MigrateDatabaseToLatestVersion<DBEntities, Configuration>());

            //关闭了实体验证
            this.Configuration.ValidateOnSaveEnabled = false;
            //关闭延迟加载
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer<DBEntities>(null);


        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<DoctorRecipeDetail>().Property(x => x.UnitPrice).HasPrecision(18, 4);
            //modelBuilder.Entity<DoctorRecipeDetail>().Property(x => x.ChannelPrice).HasPrecision(18, 4);
            //modelBuilder.Entity<DoctorRecipeFile>().Property(x => x.Amount).HasPrecision(18, 4);
            //modelBuilder.Entity<SysDrugSalePrice>().Property(x => x.UnitPrice).HasPrecision(18, 4);
            //modelBuilder.Entity<SysDrugOrgMap>().Property(x => x.UnitPrice).HasPrecision(18, 4);
            //modelBuilder.Entity<SysDrugOrgMap>().Property(x => x.ChannelPrice).HasPrecision(18, 4);
            //modelBuilder.Entity<SysDrug>().Property(x => x.UnitPrice).HasPrecision(18, 4);
            //modelBuilder.Entity<SysDrugOrgPrice>().Property(x => x.UnitPrice).HasPrecision(18, 4);

            base.OnModelCreating(modelBuilder);
        }

        #region 事务 add by lai.rijiu 2017-6-14

        internal KMTransaction transaction;
        public KMTransaction Transaction { get { return this.transaction; } }

        public KMTransaction BeginTransaction()
        {
            if (transaction == null)
            {
                this.transaction = new KMTransaction(this, this.Database.BeginTransaction());
            }

            return this.transaction;
        }

        public KMTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (transaction == null)
            {
                this.transaction = new KMTransaction(this, this.Database.BeginTransaction(isolationLevel));
            }

            return this.transaction;
        }

        public void Commit()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        #endregion

        #region 订单
        /// <summary>
        /// 订单表
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// 交易日志
        /// </summary>
        public DbSet<OrderTradeLog> OrderTradeLogs { get; set; }

        /// <summary>
        /// 退款日志
        /// </summary>
        public DbSet<OrderRefundLog> OrderRefundLogs { get; set; }

        /// <summary>
        /// 订单折扣记录
        /// </summary>
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }

        /// <summary>
        /// 订单收件人
        /// </summary>
        public DbSet<OrderConsignee> OrderConsignees { get; set; }

        /// <summary>
        /// 订单日志
        /// </summary>
        public DbSet<OrderLog> OrderLogs { get; set; }


        /// <summary>
        /// 订单日志
        /// </summary>
        public DbSet<OrderDetail> OrderDetails { get; set; }

        #endregion

        #region Sys

        /// <summary>
        /// 系统时间消息
        /// </summary>
        public DbSet<SysEventConsume> SysEventConsumes { get; set; }


        /// <summary>
        /// 系统事件消息
        /// </summary>
        public DbSet<SysEvent> SysEvents { get; set; }

        /// <summary>
        /// 短信配置
        /// </summary>
        public DbSet<SysShortMessageTemplate> SysShortMessageTemplates { get; set; }

        /// <summary>
        /// 订单支付回调记录
        /// </summary>
        public DbSet<OrderCallbackLog> OrderCallbackLogs { get; set; }
        /// <summary>
        ///文件索引
        /// </summary>
        public DbSet<SysFileIndex> SysFileIndexs { get; set; }

        public DbSet<SysModule> SysModules { get; set; }

        public DbSet<SysDereplication> SysDereplications { get; set; }

        public DbSet<UserShortMessageLog> UserShortMessageLogs { get; set; }


        public DbSet<SysDict> SysDicts { get; set; }

        public DbSet<SysConfig> SysConfigs { get; set; }

        public DbSet<SysAccessAccount> SysAccessAccounts { get; set; }



        /// <summary>
        /// 地区
        /// </summary>
        public DbSet<Region> Regions { get; set; }


        #endregion

        #region Hospital


        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<HospitalDepartment> HospitalDepartments { get; set; }

        public DbSet<HospitalWorkingTime> HospitalWorkingTimes { get; set; }



        #endregion

        #region Doctors


        public DbSet<Doctor> Doctors { get; set; }


        /// <summary>
        /// 医生服务
        /// </summary>
        public DbSet<DoctorService> DoctorServices { get; set; }


        /// <summary>
        /// 医生排版
        /// </summary>
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }


        /// <summary>
        ///医生的患者 
        /// </summary>
        public DbSet<DoctorMember> DoctorMembers { get; set; }



        #endregion

        #region Users

        /// <summary>
        /// 病历
        /// </summary>
        public DbSet<UserMedicalRecord> UserMedicalRecords { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 用户信息扩展
        /// </summary>
        public DbSet<UserExtend> UserExtends { get; set; }

        /// <summary>
        /// 微信用户绑定表
        /// </summary>
        public DbSet<UserWechatMap> UserWechatMaps { get; set; }

        /// <summary>
        /// 用户家庭成员
        /// </summary>
        public DbSet<UserMember> UserMembers { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// 用户-角色关系
        /// </summary>
        public DbSet<UserRoleMap> UserRoleMaps { get; set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        public DbSet<UserRolePrevilege> UserRolePrevileges { get; set; }


        /// <summary>
        /// 用户预约记录
        /// </summary>
        public DbSet<UserOPDRegister> UserOpdRegisters { get; set; }

        public DbSet<UserFile> UserFiles { get; set; }

        /// <summary>
        /// 电子病历
        /// </summary>
        public DbSet<UserMemberEMR> UserMemberEMRs { get; set; }

        public DbSet<UserLoginLog> UserLoginLogs { get; set; }


        #endregion

        #region 监控

        public DbSet<SysMonitorIndex> SysMonitorIndexs { get; set; }

        #endregion

        #region IM、视频房间会话

        /// <summary>
        ///会话房间
        /// </summary>
        public DbSet<ConversationRoomUid> ConversationRoomUids { get; set; }


        /// <summary>
        ///好友
        /// </summary>
        public DbSet<ConversationFriend> ConversationFriends { get; set; }

        ///// <summary>
        ///// 日志
        ///// </summary>
        public DbSet<ConversationRoomLog> ConversationRoomLogs { get; set; }

        /// <summary>
        ///会话房间
        /// </summary>
        public DbSet<ConversationRoom> ConversationRooms { get; set; }

        public DbSet<ConversationIMUid> ConversationIMUids { get; set; }

        /// <summary>
        ///会话内容
        /// </summary>
        public DbSet<ConversationMessage> ConversationMessages { get; set; }

        /// <summary>
        ///会话文件
        /// </summary>
        public DbSet<ConversationRecording> ConversationRecordings { get; set; }




        #endregion

        #region 服务评价

        /// <summary>
        ///服务评价
        /// </summary>
        public DbSet<ServiceEvaluation> ServiceEvaluations { get; set; }
        /// <summary>
        ///服务评价标签
        /// </summary>
        public DbSet<ServiceEvaluationTag> ServiceEvaluationTags { get; set; }
        #endregion

        #region 事件发布

        SortedSet<SysEvent> AttachEvents = new SortedSet<SysEvent>();

        /// <summary>
        /// 附加事件
        
        /// 日期：2017年8月30日
        /// </summary>
        /// <param name="eventObj"></param>
        public void TriggerEvent<T>(T eventObj)
            where T : XuHos.EventBus.IEvent
        {
            var eventModel = new SysEvent(eventObj, eventObj.EventId);
            this.SysEvents.Add(eventModel);
            this.AttachEvents.Add(eventModel);
        }

        void PublishEvent()
        {
            if (AttachEvents.Count > 0)
            {
                try
                {
                    using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                    {
                        var dict = AttachEvents.ToDictionary(a => a.EventID, a => new Dictionary<string, string>() {

                                { "Body",a.EventObject},
                                { "EventTypeName",a.RouteKey}
                        });

                        mqChannel.Publish(dict, (EventIds) =>
                        {

                            this.SysEvents.Where(a => !a.Enqueued && EventIds.Contains(a.EventID)).Update(a => new SysEvent()
                            {
                                Enqueued = true
                            });

                        }, (EventIds) =>
                        {

                            this.SysEvents.Where(a => !a.Enqueued && EventIds.Contains(a.EventID)).Update(a => new SysEvent()
                            {
                                Enqueued = false
                            });
                        }, (EventIds) =>
                         {
                             this.SysEvents.Where(a => !a.Enqueued && EventIds.Contains(a.EventID)).Update(a => new SysEvent()
                             {
                                 Enqueued = false
                             });

                         });
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public override int SaveChanges()
        {
            var ret = base.SaveChanges();

            PublishEvent();

            return ret;
        }
        #endregion
    }
}




