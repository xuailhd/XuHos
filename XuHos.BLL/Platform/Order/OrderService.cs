using EntityFramework.Extensions;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.DTO.Platform;
using XuHos.Entity;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XuHos.Common.Cache;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace XuHos.BLL
{
    /// <summary>
    /// 订单服务
    
    /// 日期：2016年7月29日
    /// </summary>
    public class OrderService : Common.CommonBaseService<Entity.Order>
    {
        #region 流水号
        static string _SeqPrefix = null;
        static string SeqPrefix
        {
            get
            {
                if (_SeqPrefix==null)
                {
                    var config = XuHos.BLL.Sys.Implements.SysConfigService.Get<XuHos.Common.Config.Sections.Pay>();
                    _SeqPrefix = config.SeqPrefix;
                }

                return _SeqPrefix;
            }
        }

        /// <summary>
        /// 获取全局随机数
        /// </summary>
        static int SeqOrderNo(string Suffix)
        {
            if (XuHos.Common.Cache.Manager.Instance != null)
            {
                var no = (int)(XuHos.Common.Cache.Manager.Instance.StringIncrement("SEQ:OrderNo:" + Suffix));
                XuHos.Common.Cache.Manager.Instance.ExpireEntryAt("SEQ:OrderNo:" + Suffix, DateTime.Now.AddMinutes(2) - DateTime.Now);
                return no;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取全局随机数
        /// </summary>
        static int SeqRefundNo(string Suffix)
        {
            if (XuHos.Common.Cache.Manager.Instance != null)
            {
                var no = (int)(XuHos.Common.Cache.Manager.Instance.StringIncrement("SEQ:RefundNo:" + Suffix));
                XuHos.Common.Cache.Manager.Instance.ExpireEntryAt("SEQ:RefundNo:" + Suffix, DateTime.Now.AddMinutes(2) - DateTime.Now);
                return no;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 创建订单号(18位)
        /// </summary>
        /// <param name="productType">商品类型</param>
        /// <returns></returns>
        string CreateOrderNo(EnumProductType productType)
        {
            //前缀精确到分钟，如果缓存也不会对业务超出多大影响
            var Prefix = DateTime.Now.ToString("yyyyMMddHHmm");
            //获取种子
            var Seq = SeqOrderNo(Prefix);
            //获取8为序号，不够的前面填充0
            var Suffix = Seq.ToString().PadLeft(4, '0');

            string orderNo = string.Empty;

            switch (productType)
            {
                case EnumProductType.Registration:
                    {
                        orderNo = $"{SeqPrefix}GH{Prefix}{Suffix}";
                        break;
                    }
                case EnumProductType.ImageText:
                    {
                        orderNo = $"{SeqPrefix}TW{Prefix}{Suffix}";
                        break;
                    }
                case EnumProductType.Phone:
                    {
                        orderNo = $"{SeqPrefix}DH{Prefix}{Suffix}";                     
                        break;
                    }
                case EnumProductType.video:
                    {
                        orderNo = $"{SeqPrefix}SP{Prefix}{Suffix}";                  
                        break;
                    }
                case EnumProductType.Recipe:
                    {
                        orderNo = $"{SeqPrefix}CF{Prefix}{Suffix}";
                    
                        break;
                    }
                case EnumProductType.UserPackage:
                    {
                        orderNo = $"{SeqPrefix}UP{Prefix}{Suffix}";                     
                        break;
                    }
                case EnumProductType.FamilyDoctor:
                    {
                        orderNo = $"{SeqPrefix}FD{Prefix}{Suffix}";

                       
                        break;
                    }
                case EnumProductType.UserRecharge:
                    {
                        orderNo = $"{SeqPrefix}CZ{Prefix}{Suffix}";
                      
                        break;
                    }
                case EnumProductType.MasterTechnique:
                    {
                        orderNo = $"{SeqPrefix}MT{Prefix}{Suffix}";
                        break;
                    }
                case EnumProductType.RenewUpgrade:
                    {
                        orderNo = $"{SeqPrefix}XF{Prefix}{Suffix}";
                        break;
                    }
                default:
                    {
                        orderNo = $"{SeqPrefix}KM{Prefix}{Suffix}";
                        break;
                    }
            }
            return orderNo;
        }

        #endregion

        /// <summary>
        /// 设置当前操作的用户编号
        /// </summary>
        /// <param name="CurrentOperatorUserID"></param>
        public OrderService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        {
        }

     

        #region  订单管理

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public DTO.Platform.OrderDTO CreateOrder(DTO.Platform.OrderDTO order, DBEntities db = null)
        {

            var dbPrivateScope = false;
            if (db == null)
            {
                db = new DBEntities();
                dbPrivateScope = true;
            }

            if (order.UserID == "")
            {
                order.UserID = CurrentOperatorUserID;
            }

            #region VIP6用户不需要付费
            var roleType = (from rolemap in db.UserRoleMaps
                            join role in db.UserRoles on rolemap.RoleID equals role.RoleID
                            where (role.RoleType == EnumRoleType.Vip6 || role.RoleType == EnumRoleType.DrugTreatment) && !rolemap.IsDeleted && !role.IsDeleted && rolemap.UserID == order.UserID
                            select rolemap.UserRoleMapID).FirstOrDefault();

            //VIP6等级的用户不需要付费
            if (!string.IsNullOrEmpty(roleType))
            {
                order.TotalFee = 0;
            }
            #endregion



            #region  如果订单价格是小于0则默认完成支付
            if (order.TotalFee <= 0)
            {
                order.OrderState = EnumOrderState.Paid;
                order.PayType = EnumPayType.None;
                order.TradeNo = "";
                order.TradeTime = DateTime.Now;
            }
            else
            {
                order.OrderState = EnumOrderState.NoConfirm;
            }
            #endregion

            Order entity = null;

            if (order.OrderType == EnumProductType.RenewUpgrade)
            {
                //续费订单(一个订单可以续费多次，所以OrderOutID是允许重复的)
            }
            else if (order.OrderType == EnumProductType.MasterTechnique || order.OrderType == EnumProductType.PersonalDoctor)
            {
                //大师手法
                //如果订单还未被确认可以被重新提交,返回原订单号
                entity = db.Set<Order>().Where(a => a.OrderOutID == order.OrderOutID
                    && (a.OrderState == EnumOrderState.NoConfirm || a.OrderState == EnumOrderState.NoPay)).FirstOrDefault();
            }
            else
            {
                //如果订单还未被确认可以被重新提交，否则提示订单存在
                entity = db.Set<Order>().Where(a => a.OrderOutID == order.OrderOutID).FirstOrDefault();
            }

            //通过外部关联的订单好查询发现，订单是不存在则创建
            if (entity == null)
            {
                order.OrderTime = DateTime.Now;

                //最小订单过期时间是30分钟
                if (order.OrderExpireTime < order.OrderTime.AddMinutes(30))
                {
                    order.OrderExpireTime = order.OrderTime.AddMinutes(30);
                }

                order.OrderNo = CreateOrderNo(order.OrderType);
                entity = new Order();
                entity.OrderNo = order.OrderNo;
                entity.CreateTime = order.OrderTime;
                entity.OrderTime = order.OrderTime;
                entity.OrderExpireTime = order.OrderExpireTime;
                entity.CreateUserID = CurrentOperatorUserID;
                entity.IsDeleted = false;
                entity.UserID = order.UserID;
                entity.OrderOutID = order.OrderOutID;
                entity.OrderType = order.OrderType;
                entity.LogisticCompanyName = "";
                entity.LogisticWayBillNo = "";

                entity.TradeNo = "";//交易编号（支付完成后才有）
                if (order.TradeTime.HasValue)
                {
                    entity.TradeTime = order.TradeTime.Value;
                }
                entity.totalFee = order.TotalFee;//实际交易价格
                entity.OriginalPrice = order.TotalFee;//原始价格
                entity.RefundFee = 0;//退款金额（退款后才有）
                entity.RefundState = EnumRefundState.NoRefund;//未退款
                entity.RefundNo = "";
                entity.OrderState = order.OrderState;
                entity.LogisticNo = "";
                entity.LogisticState = EnumLogisticState.待审核;
                entity.CancelReason = "";
                entity.SellerID = "";
                entity.OrgnazitionID = order.OrgnazitionID;
                entity.CostType = order.CostType;


                //设置订单详情
                foreach (var item in order.Details)
                {
                    OrderDetail detailEntity = new OrderDetail();
                    detailEntity.Body = item.Body + "";
                    detailEntity.CreateTime = DateTime.Now;
                    detailEntity.CreateUserID = entity.UserID;
                    detailEntity.Discount = item.Discount;
                    detailEntity.Fee = item.Fee;
                    detailEntity.GroupNo = 0;
                    detailEntity.IsDeleted = false;
                    detailEntity.OrderNO = entity.OrderNo;
                    detailEntity.ProductId = item.ProductId;
                    detailEntity.ProductType = item.ProductType;
                    detailEntity.Quantity = item.Quantity;
                    detailEntity.Remarks = "";
                    detailEntity.Subject = item.Subject;
                    detailEntity.UnitPrice = item.UnitPrice;
                    detailEntity.OrderDetailID = Guid.NewGuid().ToString();
                    db.Set<OrderDetail>().Add(detailEntity);

                }

                db.Set<Order>().Add(entity);

                //写订单日志
                db.OrderLogs.Add(new OrderLog()
                {
                    Remark = entity.OrderType.GetEnumDescript(),
                    CreateTime = DateTime.Now,
                    OperationDesc = EnumEnumOrderLogOperationType.CreateOrder.GetEnumDescript(),
                    OperationTime = DateTime.Now,
                    OperationType = EnumEnumOrderLogOperationType.CreateOrder,
                    OperationUserID = CurrentOperatorUserID,
                    OrderNo = entity.OrderNo,
                    OrderLogID = Guid.NewGuid().ToString("N"),
                    IsDeleted = false
                });

                if (entity.OrderState == EnumOrderState.Paid)
                {
                    //写订单日志
                    db.OrderLogs.Add(new OrderLog()
                    {
                        Remark = order.OrderType.GetEnumDescript(),
                        CreateTime = DateTime.Now,
                        OperationDesc = EnumEnumOrderLogOperationType.OrderPayCompleted.GetEnumDescript(),
                        OperationTime = DateTime.Now,
                        OperationType = EnumEnumOrderLogOperationType.OrderPayCompleted,
                        OperationUserID = CurrentOperatorUserID,
                        OrderNo = order.OrderNo,
                        OrderLogID = Guid.NewGuid().ToString("N"),
                        IsDeleted = false
                    });
                }
            }
            else
            {
                order.OrderNo = entity.OrderNo;
            }
            if (dbPrivateScope)
            {
                db.SaveChanges();
            }

            return order;
        }

        public string GetOrderNOByOrderOutID(string OrderOutID)
        {
            var OrderNo = "";

            if (!string.IsNullOrEmpty(OrderOutID))
            {
                var CacheKey_MAP_GetOrderNoByOrderOutID = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetOrderNoByOrderOutID, OrderOutID);

                OrderNo = CacheKey_MAP_GetOrderNoByOrderOutID.FromCache<string>();

                if (OrderNo == null)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        OrderNo = db.Set<Order>().Where(a => a.OrderOutID == OrderOutID).Select(a => a.OrderNo).FirstOrDefault();
                        OrderNo.ToCache(CacheKey_MAP_GetOrderNoByOrderOutID,TimeSpan.FromHours(1));
                    }
                }
            }

            return OrderNo;
        }

        /// <summary>
        /// 请求付款
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="trade"></param>
        /// <returns></returns>
        public DTO.Platform.OrderDTO GetOrder(string OrderNo, string OrderOutID = "")
        {
            var order = new DTO.Platform.OrderDTO();

            //订单编号是空，则通过外部订单编号获取订单号
            if (string.IsNullOrEmpty(OrderNo))
            {
                OrderNo = GetOrderNOByOrderOutID(OrderOutID);
            }

            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNo);
            order = CacheKey_Order.FromCache();

            //缓存没有命中，从数据库获取
            if (order == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    Order entity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();

                    if (entity != null)
                    {
                        order = new DTO.Platform.OrderDTO();
                        //订单编号
                        order.OrderNo = entity.OrderNo;
                        order.OrderState = entity.OrderState;
                        order.OrderTime = entity.OrderTime;//订单时间
                        order.TradeNo = entity.TradeNo;//交易编号
                        order.TradeTime = entity.TradeTime;//交易时间
                        order.PayType = entity.PayType;
                        order.CostType = entity.CostType;
                        order.OrderType = entity.OrderType;
                        order.OrderOutID = entity.OrderOutID;
                        order.LogisticNo = entity.LogisticNo;//物流编号
                        order.LogisticState = entity.LogisticState;
                        order.CancelTime = entity.CancelTime;//订单取消时间
                        order.FinishTime = entity.FinishTime;//订单完成时间
                        order.ExpressTime = entity.ExpressTime;//发货时间
                        order.StoreTime = entity.StoreTime;//出库时间
                        order.RefundFee = entity.RefundFee;
                        order.RefundState = entity.RefundState;
                        order.RefundTime = entity.RefundTime;
                        order.OrderExpireTime = entity.OrderExpireTime;//订单过期时间
                        order.CancelReason = entity.CancelReason;//取消原因
                        order.Details = new List<OrderDetailDTO>();
                        order.Consignee = new OrderConsigneeDTO();
                        order.UserID = entity.UserID;
                        order.SellerID = entity.SellerID;
                        order.RefundNo = entity.RefundNo;
                        order.OrgnazitionID = entity.OrgnazitionID;
                        order.OriginalPrice = entity.OriginalPrice;
                        order.LogisticCompanyName = entity.LogisticCompanyName;
                        order.LogisticWayBillNo = entity.LogisticWayBillNo;
                        order.TotalFee = entity.totalFee;
                        order.IsEvaluated = entity.IsEvaluated;

                        var orderDetails = db.Set<OrderDetail>().Where(a => a.OrderNO == OrderNo);

                        //收货人
                        var orderConsignee = db.Set<OrderConsignee>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();
                        if (orderConsignee != null)
                        {
                            //收货信息
                            order.Consignee = new OrderConsigneeDTO()
                            {
                                Address = orderConsignee.ConsigneeAddress,
                                IsHosAddress = orderConsignee.IsHosAddress,
                                SendGoodsTime = orderConsignee.SendGoodsTime,
                                Tel = orderConsignee.ConsigneeTel,
                                Name = orderConsignee.ConsigneeName
                            };
                        }

                        if (orderDetails != null)
                        {
                            //生成订单详情
                            foreach (var item in orderDetails)
                            {
                                order.Details.Add(new OrderDetailDTO()
                                {

                                    Body = item.Body,
                                    ProductId = item.ProductId,
                                    ProductType = (EnumProductType)item.ProductType,
                                    Quantity = item.Quantity,
                                    Subject = item.Subject,
                                    UnitPrice = item.UnitPrice
                                });
                            }
                        }

                        order.ToCache(CacheKey_Order, TimeSpan.FromMinutes(5));
                    }
                }
            }

            return order;
        }



        /// <summary>
        /// 支付完成
        
        /// 日期：2016年12月26日
        /// </summary>
        /// <param name="OrderNO">订单编号</param>
        /// <param name="TradeNo">交易流水（微信、支付宝交易编号）</param>
        /// <param name="payType">支付类型（微信、支付宝、康美钱包）</param>
        /// <returns></returns>
        public bool PayCompleted(string OrderNO,
            string TradeNo,
            EnumPayType payType,
            string SellerID = "",
            EnumTradeState TradeState = EnumTradeState.TRADE_SUCCESS,
            DBEntities db = null)
        {
            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNO);

            var dbPrivateScope = false;
            var ret = false;
            if (db == null)
            {
                db = new DBEntities();
                dbPrivateScope = true;
            }

      
                //获取订单
                var orderEntity = db.Set<Order>().Where(a => a.OrderNo == OrderNO).FirstOrDefault();

                //订单存在
                if (orderEntity != null)
                {
                    //获取订单折扣
                    var discountEntity = db.Set<OrderDiscount>().Where(a => a.OrderNo == OrderNO && a.State == EnumDiscountConsumeState.NoPay).FirstOrDefault();

                    var tradeLogEntity = db.Set<OrderTradeLog>().Where(a => a.OrderNo == OrderNO && a.PayType == payType).FirstOrDefault();


                    if (string.IsNullOrEmpty(orderEntity.TradeNo))
                    {
                        orderEntity.PayType = payType;
                        orderEntity.TradeNo = TradeNo;
                        orderEntity.OrderState = EnumOrderState.Paid;
                        orderEntity.ModifyTime = DateTime.Now;
                        orderEntity.TradeTime = DateTime.Now;
                        orderEntity.SellerID = SellerID;
                        db.Update(orderEntity);
                    }

                    if (tradeLogEntity == null)
                    {
                        //写交易日志日志
                        db.OrderTradeLogs.Add(new OrderTradeLog()
                        {
                            PayType = payType,//支付方式
                            TradeTime = DateTime.Now,//交易日期       
                            CreateTime = DateTime.Now,//创建时间
                            OrderNo = orderEntity.OrderNo,//订单好
                            TradeNo = TradeNo,//交易编号
                            SellerID = SellerID,//收款展会高
                            TradeStatus = TradeState,
                            TradeFee = orderEntity.totalFee,//交易基恩
                            IsDeleted = false

                        });
                    }
                    else
                    {
                        tradeLogEntity.TradeStatus = TradeState;
                        tradeLogEntity.TradeNo = TradeNo;
                        tradeLogEntity.SellerID = SellerID;
                        db.Update(tradeLogEntity);
                    }

                    //写订单日志
                    db.OrderLogs.Add(new OrderLog()
                    {
                        Remark = $"交易类型：{orderEntity.OrderType.GetEnumDescript()},交易编号:{TradeNo},收款账号：{SellerID},金额:{orderEntity.totalFee}",
                        CreateTime = DateTime.Now,
                        OperationDesc = EnumEnumOrderLogOperationType.OrderPayCompleted.GetEnumDescript(),
                        OperationTime = DateTime.Now,
                        OperationType = EnumEnumOrderLogOperationType.OrderPayCompleted,
                        OperationUserID = CurrentOperatorUserID,
                        OrderNo = orderEntity.OrderNo,
                        OrderLogID = Guid.NewGuid().ToString("N"),
                        IsDeleted = false
                    });

                    db.TriggerEvent(new XuHos.EventBus.Events.OrderPayCompletedEvent()
                    {
                        OrderNo = OrderNO,
                        PayType = payType,
                        OrderType = orderEntity.OrderType,
                        PayPrivilege = discountEntity != null ? discountEntity.Privilege : EnumPayPrivilege.None,
                        OrderOutID = orderEntity.OrderOutID,
                        TradeNo = orderEntity.TradeNo

                    });

                 

                    if (dbPrivateScope)
                    {
                        ret = db.SaveChanges() > 0;
                    }
                    else
                    {
                        ret = true;
                    }

                    if (ret)
                    {
                        CacheKey_Order.RemoveCache();
                     
                    }

                }
                else
                {
                    ret = true;
                }
            

            return ret;


        }

        /// <summary>
        /// 支付完成
        
        /// 日期：2016年12月26日
        /// </summary>
        /// <param name="OrderNO">订单编号</param>
        /// <param name="TradeNo">交易流水（微信、支付宝交易编号）</param>
        /// <param name="payType">支付类型（微信、支付宝、康美钱包）</param>
        /// <returns></returns>
        public bool SetTradeLog(string OrderNo, EnumPayType payType, EnumTradeState TradeStatus, string TradeNo, decimal TradeFee, string SellerID)
        {
            using (DBEntities db = new DBEntities())
            {
                var tradeLogEntity = db.Set<OrderTradeLog>().Where(a => a.OrderNo == OrderNo && a.PayType == payType).FirstOrDefault();

                if (tradeLogEntity == null)
                {
                    //写交易日志日志
                    db.OrderTradeLogs.Add(new OrderTradeLog()
                    {
                        PayType = payType,//支付方式
                        TradeTime = DateTime.Now,//交易日期       
                        CreateTime = DateTime.Now,//创建时间
                        ModifyTime = DateTime.Now,
                        OrderNo = OrderNo,//订单好
                        TradeNo = TradeNo,//交易编号
                        SellerID = SellerID,//收款展会高
                        TradeStatus = TradeStatus,
                        TradeFee = TradeFee,//交易基恩
                        IsDeleted = false
                    });
                }
                else
                {
                    if (tradeLogEntity.TradeStatus == EnumTradeState.WAIT_BUYER_PAY)
                    {
                        tradeLogEntity.TradeStatus = TradeStatus;
                        tradeLogEntity.TradeNo = TradeNo;
                        tradeLogEntity.SellerID = SellerID;
                        tradeLogEntity.TradeFee = TradeFee;
                        tradeLogEntity.ModifyTime = DateTime.Now;
                        db.Update(tradeLogEntity);
                    }
                    else
                    {
                        return true;
                    }
                }

                return db.SaveChanges() > 0;
            }
        }


        /// <summary>
        /// 取消订单
        
        /// 日期：2016年12月26日
        /// </summary>
        /// <param name="OrderNO">订单编号</param>
        /// <param name="RefundNo"></param>
        /// <returns></returns>
        public bool Cancel(string OrderNO, string CancelReason = "暂无", decimal? RefundFee = null, DBEntities db = null)
        {
            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNO);

            var ret = false;
            var UserId = CurrentOperatorUserID;
            var dbPrivateScope = false;

            if (db == null)
            {
                db = new DBEntities();
                dbPrivateScope = true;
            }

            Order entity = new Order();

            entity = db.Set<Order>().Where(a => a.OrderNo == OrderNO).FirstOrDefault();

            //取消没有确认或没有支付的订单
            if (entity != null)
            {
                //没有指定退款金额则全额退款
                if (!RefundFee.HasValue)
                {
                    RefundFee = entity.totalFee;
                }

                //退款金额不能大于总支付的金额
                if (RefundFee > entity.totalFee)
                {
                    RefundFee = entity.totalFee;
                }

                //如果物流状态是（已发货、配送中、已经送达）则不允许取消订单
                if (entity.LogisticState == EnumLogisticState.已发货 ||
                    entity.LogisticState == EnumLogisticState.配送中 ||
                    entity.LogisticState == EnumLogisticState.已送达)
                {
                    XuHos.Common.LogHelper.WriteInfo("订单" + OrderNO + " 取消失败，状态 “" + entity.LogisticState.GetEnumDescript() + "”");
                    ret = false;
                }
                else
                {
                    #region 退库存(库存退失败则不允许取消)
                    var stockService = XuHos.BLL.Platform.Order.StockFactoryService.Create(entity.OrderType);

                    if (stockService != null)
                    {
                        //调用库存，加库存操作。如果操作失败则不能取消
                        var stockRestoreResult = stockService.Restore(entity.Map<Entity.Order, DTO.Platform.OrderDTO>(), CancelReason);
                        if (stockRestoreResult != EnumStockRestoreResult.Success)
                        {
                            XuHos.Common.LogHelper.WriteWarn(string.Format("订单:{0} 取消失败", entity.OrderNo, entity.PayType.GetEnumDescript()));
                            return false;
                        }
                        else
                        {
                            db.TriggerEvent(new EventBus.Events.StockRefundedEvent()
                            {
                                OrderNo = entity.OrderNo,
                                OrderOutID = entity.OrderOutID,
                                OrderType = entity.OrderType
                            });
                        }
                    }
                    #endregion

                    if (entity.OrderState == EnumOrderState.NoPay || entity.OrderState == EnumOrderState.NoConfirm)
                    {
                        db.TriggerEvent(new EventBus.Events.OrderCanceledEvent()
                        {
                            OrderNo = entity.OrderNo
                        });

                        #region 修改订单信息
                        entity.RefundNo = "";//退款单号
                        entity.RefundFee = RefundFee.Value;
                        entity.RefundTime = DateTime.Now;
                        entity.ModifyTime = DateTime.Now;
                        entity.OrderState = EnumOrderState.Canceled;
                        entity.ModifyTime = DateTime.Now;
                        entity.CancelTime = DateTime.Now;
                        entity.CancelReason = CancelReason;
                        db.Update(entity);

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.OrderType.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.CancelOrder.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.CancelOrder,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        if (dbPrivateScope)
                        {
                            ret = db.SaveChanges() > 0;
                        }
                        else
                        {
                            ret = true;
                        }
                        #endregion
                    }
                    else if (entity.OrderState == EnumOrderState.Paid)
                    {
                        db.TriggerEvent(new EventBus.Events.OrderCanceledEvent()
                        {
                            OrderNo = entity.OrderNo
                        });

                        #region 物流状态变更日志
                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = EnumLogisticState.已取消.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.LogisticStateChanged.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.LogisticStateChanged,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });
                        #endregion

                        #region 修改订单信息
                        entity.RefundNo = "";//退款单号
                        entity.RefundFee = RefundFee.Value;
                        entity.RefundState = EnumRefundState.applyRefund;
                        entity.RefundTime = DateTime.Now;
                        entity.ModifyTime = DateTime.Now;
                        entity.OrderState = EnumOrderState.Canceled;
                        entity.CancelReason = CancelReason;
                        entity.CancelTime = DateTime.Now;
                        entity.LogisticState = EnumLogisticState.已取消;
                        db.Update(entity);

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.OrderType.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.RefundApply.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.RefundApply,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.OrderType.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.CancelOrder.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.CancelOrder,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        if (dbPrivateScope)
                        {
                            ret = db.SaveChanges() > 0;
                        }
                        else
                        {
                            ret = true;
                        }
                        #endregion
                    }
                    else if (entity.OrderState == EnumOrderState.Canceled)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }
                }
            }
            else
            {
                XuHos.Common.LogHelper.WriteWarn("订单" + OrderNO + "不存在");
            }

            //事务消息提交
            if (ret)
            {
                //移除缓存
                CacheKey_Order.RemoveCache();
            }

            return ret;


        }

        /// <summary>
        /// 退款（已申请退款）
        
        /// 日期：2016年10月17日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public bool Refund(string OrderNo, EnumPayType PayType, string TradeNo)
        {
            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNo);
            var RefundNo = TradeNo;

            if (PayType == EnumPayType.AliPay)
            {
                RefundNo = DateTime.Now.ToString("yyyyMMdd") + TradeNo.Substring(8, TradeNo.Length - 8);
            }

            using (DBEntities db = new DBEntities())
            {
                var entity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();

                var tradeLogEntity = db.Set<OrderTradeLog>().Where(a => a.OrderNo == OrderNo && a.PayType == PayType && a.TradeNo == TradeNo).FirstOrDefault();// && a.OnlineTransactionNo != ""

                //订单存在且已经取消，交易日志存在才能够退款
                if (entity != null && tradeLogEntity != null && entity.OrderState == EnumOrderState.Canceled)
                {
                    var cashierService = XuHos.BLL.Platform.Cashier.CashierFactoryService.Create(PayType, CurrentOperatorUserID);

                    if (cashierService != null)
                    {
                        var refundLogEntity = db.Set<OrderRefundLog>().Where(a => a.OrderNo == OrderNo && a.PayType == PayType).FirstOrDefault();

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.OrderType.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.Refunding.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.Refunding,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        //没有退款日志是写日志
                        if (refundLogEntity == null)
                        {
                            refundLogEntity = new OrderRefundLog()
                            {
                                OrderNo = OrderNo,//订单好
                                TradeNo = TradeNo,//交易编号
                                PayType = PayType,//支付方式
                                RefundNo = RefundNo,
                                RefundFee = entity.RefundFee,
                                RefundState = EnumRefundState.Refunding,
                                RefundTime = DateTime.Now,//交易日期       
                                CreateTime = DateTime.Now,//创建时间
                                IsDeleted = false
                            };

                            //退款日志
                            db.OrderRefundLogs.Add(refundLogEntity);

                        }
                        //重复退款的情况
                        else if (refundLogEntity.RefundState == EnumRefundState.applyRefund)
                        {
                            if (string.IsNullOrEmpty(refundLogEntity.RefundNo))
                            {
                                refundLogEntity.RefundNo = RefundNo;
                            }
                            refundLogEntity.RefundState = EnumRefundState.Refunding;
                            db.Update(refundLogEntity);
                        }

                        #region 修改订单表信息
                        entity.RefundState = refundLogEntity.RefundState;
                        entity.RefundNo = refundLogEntity.RefundNo;
                        db.Update(entity);
                        #endregion

                        //开始退款
                        if (cashierService.ApplyRefund(
                            OrderNo: OrderNo,
                            SellerID: tradeLogEntity.SellerID,
                            TradeNo: tradeLogEntity.TradeNo,
                            TotalFee: tradeLogEntity.TradeFee,
                            RefundNo: refundLogEntity.RefundNo,
                            RefundFee: refundLogEntity.RefundFee,
                            OnlineTransactionNo: tradeLogEntity.OnlineTransactionNo))
                        {
                            //日志写入成功
                            if (db.SaveChanges() > 0)
                            {
                                CacheKey_Order.RemoveCache();

                                //申请退款后是否退款就立即完成（如果是则无需等待第三方平台通知）
                                if (cashierService.IsNotWaitRefundNotifyIfApplyRefund(OrderNo, PayType))
                                {
                                    return RefundCompleted(OrderNo, PayType);
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            XuHos.Common.LogHelper.WriteWarn($"{PayType.GetEnumDescript()}订单 {OrderNo}:退款失败");
                        }
                    }
                    else
                    {
                        XuHos.Common.LogHelper.WriteWarn(string.Format("订单:{0} 不支持{1}类型的退款", OrderNo, PayType.GetEnumDescript()));
                        return false;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// 完成退款
        
        /// 日期：2016年12月26日
        /// </summary>
        /// <param name="OrderNo">订单编号</param>
        /// <returns></returns>
        public bool RefundCompleted(string OrderNo, EnumPayType PayType, DBEntities db = null)
        {
            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNo);

            var dbPrivateScope = false;
            var ret = false;

            if (db == null)
            {
                db = new DBEntities();
                dbPrivateScope = true;
            }

            Order entity = new Order();

            entity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();

            var refundLog = db.Set<OrderRefundLog>().Where(a => a.OrderNo == OrderNo && a.PayType == PayType).FirstOrDefault();

            if (entity != null)
            {
                //订单已经取消（已申请退款或正在退款的修改状态为完成）
                if (entity.OrderState == EnumOrderState.Canceled && (entity.RefundState == EnumRefundState.applyRefund || entity.RefundState == EnumRefundState.Refunding))
                {
                    entity.RefundState = EnumRefundState.Refunded;
                    entity.RefundTime = DateTime.Now;
                    entity.ModifyTime = DateTime.Now;
                    db.Update(entity);

                    //写订单日志
                    db.OrderLogs.Add(new OrderLog()
                    {
                        Remark = entity.OrderType.GetEnumDescript(),
                        CreateTime = DateTime.Now,
                        OperationDesc = EnumEnumOrderLogOperationType.RefundCompleted.GetEnumDescript(),
                        OperationTime = DateTime.Now,
                        OperationType = EnumEnumOrderLogOperationType.RefundCompleted,
                        OperationUserID = CurrentOperatorUserID,
                        OrderNo = entity.OrderNo,
                        OrderLogID = Guid.NewGuid().ToString("N"),
                        IsDeleted = false
                    });
                }

                //退款日志存在，退款日志中退款状态是
                if (refundLog != null && (refundLog.RefundState == EnumRefundState.applyRefund || refundLog.RefundState == EnumRefundState.Refunding))
                {
                    refundLog.RefundState = EnumRefundState.Refunded;
                    refundLog.ModifyTime = DateTime.Now;
                    db.Update(refundLog);
                }

                if (dbPrivateScope)
                {
                    ret = db.SaveChanges() >= 0;
                }
                else
                {
                    ret = true;
                }
            }

            if (ret)
                CacheKey_Order.RemoveCache();


            return ret;

        }

        /// <summary>
        /// 确认订单
        
        /// 日期：2016年12月26日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public EnumApiStatus Confirm(string OrderNo, RequestOrderConfirmDTO request, DBEntities db = null)
        {
            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNo);
            var ret = EnumApiStatus.BizError;
            var dbPrivateScope = false;
            var UserId = CurrentOperatorUserID;

            if (db == null)
            {
                db = new DBEntities();
                dbPrivateScope = true;
            }

            Order orderEntity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();

            if (orderEntity != null)
            {
                //注意：已经取消的订单允许重新确认
                if (orderEntity.OrderState == EnumOrderState.NoConfirm
                    || orderEntity.OrderState == EnumOrderState.NoPay
                    || orderEntity.OrderState == EnumOrderState.Canceled)
                {

                    //设置订单状态
                    orderEntity.OrderState = EnumOrderState.NoPay;
                    orderEntity.OrderTime = DateTime.Now;

                    #region 更新收货人信息
                    if (request != null && request.Consignee != null)
                    {
                        //获取当前订单设置的收货人
                        var orderConsignee = db.Set<OrderConsignee>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();

                      

                       #region 检查物流配送的信息是否齐全
                        //如果是处方订单则需要检测收货人设置是否正确
                        if (orderEntity.OrderType == EnumProductType.Recipe)
                        {
                            if (string.IsNullOrEmpty(request.Consignee.Address))
                            {
                                return EnumApiStatus.BizOrderRejectConfirmIfConsigneeInvalid;
                            }

                            if (string.IsNullOrEmpty(request.Consignee.Name))
                            {
                                return EnumApiStatus.BizOrderRejectConfirmIfConsigneeInvalid;
                            }

                            if (string.IsNullOrEmpty(request.Consignee.Tel))
                            {
                                return EnumApiStatus.BizOrderRejectConfirmIfConsigneeInvalid;
                            }
                        }
                        #endregion

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = $"姓名:{request.Consignee.Name}、电话：{ request.Consignee.Address}、地址:{request.Consignee.Address}",
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.LogisticInfoChanged.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.LogisticInfoChanged,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = orderEntity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        //还没有设置收货人，则新增收货人
                        if (orderConsignee == null)
                        {
                            #region 新增信息
                            orderConsignee = new OrderConsignee();
                            orderConsignee.OrderNo = OrderNo;
                            orderConsignee.ConsigneeAddress = request.Consignee.Address + "";//地址
                            orderConsignee.ConsigneeName = request.Consignee.Name + "";//姓名
                            orderConsignee.ConsigneeTel = request.Consignee.Tel + "";//电话
                            orderConsignee.ModifyTime = DateTime.Now;//修改时间
                            orderConsignee.ModifyUserID = UserId;//修噶人
                            orderConsignee.SendGoodsTime = request.Consignee.SendGoodsTime;
                            db.Set<OrderConsignee>().Add(orderConsignee);
                            #endregion
                        }
                        else
                        {
                            #region 更新信息
                            orderConsignee.OrderNo = OrderNo;
                            orderConsignee.ConsigneeAddress = request.Consignee.Address + "";//地址
                            orderConsignee.ConsigneeName = request.Consignee.Name + "";//姓名
                            orderConsignee.ConsigneeTel = request.Consignee.Tel + "";//电话
                            orderConsignee.ModifyTime = DateTime.Now;//修改时间
                            orderConsignee.ModifyUserID = UserId;//修噶人
                            orderConsignee.SendGoodsTime = request.Consignee.SendGoodsTime;
                            db.Update(orderConsignee);
                            #endregion
                        }
                    }
                    else
                    {
                        //如果是处方订单则需要检测收货人设置是否正确
                        if (orderEntity.OrderType == EnumProductType.Recipe)
                        {
                            return EnumApiStatus.BizOrderRejectConfirmIfConsigneeInvalid;
                        }
                    }

                    #endregion

                    #region 检查是否需要付费（免费、义诊、套餐）
                    //不使用特权&价格为0的情况
                    if (request.Privilege == EnumPayPrivilege.None && orderEntity.totalFee == 0)
                    {
                        LogHelper.WriteDebug($"【确认订单】OrderNo: {OrderNo}, 不使用特权&价格为0的情况");
                        orderEntity.OrderState = EnumOrderState.Paid;
                    }

                    #endregion

                    db.Update(orderEntity);
                    //写订单日志
                    db.OrderLogs.Add(new OrderLog()
                    {
                        Remark = orderEntity.OrderType.GetEnumDescript(),
                        CreateTime = DateTime.Now,
                        OperationDesc = EnumEnumOrderLogOperationType.ConfirmOrder.GetEnumDescript(),
                        OperationTime = DateTime.Now,
                        OperationType = EnumEnumOrderLogOperationType.ConfirmOrder,
                        OperationUserID = CurrentOperatorUserID,
                        OrderNo = orderEntity.OrderNo,
                        OrderLogID = Guid.NewGuid().ToString("N"),
                        IsDeleted = false
                    });

                    if (orderEntity.OrderState == EnumOrderState.Paid)
                    {
                        if (!PayCompleted(OrderNo, "", EnumPayType.None, "", EnumTradeState.TRADE_SUCCESS, db))
                        {
                            return EnumApiStatus.BizError;
                        }
                    }

                    if (dbPrivateScope)
                    {
                        db.SaveChanges();
                        ret = EnumApiStatus.BizOK;
                    }
                    else
                    {
                        ret = EnumApiStatus.BizOK;
                    }
                }
                else
                {
                    if (orderEntity.OrderState == EnumOrderState.Paid)
                    {
                        if (!PayCompleted(OrderNo, "", EnumPayType.None, "", EnumTradeState.TRADE_SUCCESS))
                        {
                            return EnumApiStatus.BizError;
                        }
                    }

                    ret = EnumApiStatus.BizOK;
                }
            }
            else
            {
                return EnumApiStatus.BizOrderNotExists;
            }

            CacheKey_Order.RemoveCache();
            return ret;
        }


        /// <summary>
        /// 创建升级续费订单
        /// </summary>
        /// <param name="OrderNo">原订单编号</param>
        /// <param name="TotalFee">订单价格</param>
        /// <returns></returns>
        public OrderDTO CreateRenewUpgradeOrder(string OrderNo, decimal TotalFee)
        {
            var order = GetOrder(OrderNo);
            return CreateOrder(new OrderDTO()
            {
                OrderOutID = order.OrderNo,//续费订单的外部订单编号是订单编号
                OrderState = EnumOrderState.NoPay,
                OrderTime = DateTime.Now,
                OrderNo = "",
                OrderExpireTime = DateTime.Now.AddMinutes(2),
                OrderType = EnumProductType.RenewUpgrade,
                IsEvaluated = false,
                LogisticNo = "",
                LogisticState = EnumLogisticState.审核中,
                OriginalPrice = TotalFee,
                UserID = order.UserID,
                TotalFee = TotalFee,
                OrgnazitionID = order.OrgnazitionID,
                Details = new List<OrderDetailDTO>() {

                      new OrderDetailDTO() {

                           Body=$"{order.OrderType.GetEnumDescript()}(续费)",
                           ProductId=order.OrderOutID,
                           Quantity=1,
                           ProductType= order.OrderType,
                           Subject=order.OrderType.GetEnumDescript(),
                           UnitPrice=TotalFee,
                      }
                }

            });
        }

        /// <summary>
        /// 升级续费订单支付完成
        /// 前置条件：订单支付成功
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RenewUpgradePayCompleted(string OrderNo)
        {
            using (DBEntities db = new DBEntities())
            {
                var renewOrder = GetOrder(OrderNo);

                if (renewOrder != null)
                {
                    if (Complete(OrderNo))
                    {
                        //通过续费订单的外部订单编号找出原订单
                        var order = db.Orders.Where(a => a.OrderNo == renewOrder.OrderOutID).FirstOrDefault();


                        if (order != null)
                        {
                            //将续费订单中的明细信息写入原订单中
                            foreach (var item in renewOrder.Details)
                            {
                                OrderDetail detailEntity = new OrderDetail();
                                detailEntity.Body = item.Body;
                                detailEntity.CreateTime = DateTime.Now;
                                detailEntity.CreateUserID = order.UserID;
                                detailEntity.Discount = item.Discount;
                                detailEntity.Fee = item.Fee;
                                detailEntity.GroupNo = 0;
                                detailEntity.IsDeleted = false;
                                detailEntity.OrderNO = order.OrderNo;
                                detailEntity.ProductId = item.ProductId;
                                detailEntity.ProductType = item.ProductType;
                                detailEntity.Quantity = item.Quantity;
                                detailEntity.Remarks = "续费";
                                detailEntity.Subject = item.Subject;
                                detailEntity.UnitPrice = item.UnitPrice;
                                detailEntity.OrderDetailID = Guid.NewGuid().ToString();
                                db.Set<OrderDetail>().Add(detailEntity);
                            }


                            db.Update(order);
                            return db.SaveChanges() > 0 ? true : false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 物流（发货）
        /// 根据不同的订单类型进行业务处理
        
        /// 日期：2016年9月5日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public bool LogisticWithDelivery(string OrderNo)
        {
            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, OrderNo);

            Order entity = new Order();
            using (DBEntities db = new DBEntities())
            {
                entity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();
                if (entity != null)
                {
                    //已经交易未完成则发货
                    if (entity.OrderState == EnumOrderState.Paid &&
                        entity.LogisticState == EnumLogisticState.待审核)
                    {
                        //是否发货成功
                        var Deliveryed = false;
                        //物流单号码
                        var LogisticNo = "";

                        var LogisticState = EnumLogisticState.审核中;

                        var stockService = XuHos.BLL.Platform.Order.StockFactoryService.Create(entity.OrderType);

                        if (stockService != null)
                        {
                            Deliveryed = stockService.Delivery(entity.OrderOutID, out LogisticNo, out LogisticState);
                        }
                        else
                        {
                            Deliveryed = true;
                            LogisticNo = "-";
                        }

                        if (Deliveryed)
                        {
                            //更新物流编号和物流状态
                            if (LogisticNo != "")
                            {
                                entity.LogisticState = LogisticState;
                                entity.LogisticNo = LogisticNo;
                                entity.StoreTime = DateTime.Now;

                                //写订单日志
                                db.OrderLogs.Add(new OrderLog()
                                {
                                    Remark = entity.LogisticState.GetEnumDescript(),
                                    CreateTime = DateTime.Now,
                                    OperationDesc = EnumEnumOrderLogOperationType.LogisticStateChanged.GetEnumDescript(),
                                    OperationTime = DateTime.Now,
                                    OperationType = EnumEnumOrderLogOperationType.LogisticStateChanged,
                                    OperationUserID = CurrentOperatorUserID,
                                    OrderNo = OrderNo,
                                    OrderLogID = Guid.NewGuid().ToString("N"),
                                    IsDeleted = false
                                });

                                db.Update(entity);
                                if (db.SaveChanges() <= 0)
                                {
                                    XuHos.Common.LogHelper.WriteWarn("发货:保存数据失败");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                CacheKey_Order.RemoveCache();
                return true;
            }
        }


        /// <summary>
        /// 物流（订单配送中）
        /// 需根据不同的订单类型进行处理
        
        /// 日期：2016年9月5日
        /// </summary>
        /// <param name="OrderNO"></param>
        /// <returns></returns>
        public bool LogisticWithDistributionIn(string OrderOutID)
        {





            Order entity = new Order();
            using (DBEntities db = new DBEntities())
            {
                entity = db.Set<Order>().Where(a => a.OrderOutID == OrderOutID).FirstOrDefault();

                if (entity != null)
                {
                    //已付款，已经发货的订单
                    if (entity.OrderState == EnumOrderState.Paid
                        && (
                            entity.LogisticState == EnumLogisticState.已备货 ||
                            entity.LogisticState == EnumLogisticState.已发货))
                    {
                        entity.LogisticState = EnumLogisticState.配送中;
                        entity.ExpressTime = DateTime.Now;
                        db.Update(entity);

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.LogisticState.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.LogisticStateChanged.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.LogisticStateChanged,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        if (db.SaveChanges() <= 0)
                        {
                            return false;
                        }
                    }
                }
                var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, entity.OrderNo);
                CacheKey_Order.RemoveCache();
                return true;
            }
        }

        /// <summary>
        /// 更新徐留状态
        
        /// 日期：2016年9月5日
        /// </summary>
        /// <param name="OrderOutID"></param>
        /// <returns></returns>
        public bool LogisticStateUpdate(string OrderNo, EnumLogisticState? State=null,string LogisticCompanyName=null,string LogisticWayBillNo=null)
        {
            Order entity = new Order();
            using (DBEntities db = new DBEntities())
            {
                entity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();

                if (entity != null)
                {
                    //已付款，已经发货的订单
                    if (entity.OrderState == EnumOrderState.Paid && entity.LogisticNo != "")
                    {
                        if (State.HasValue)
                        {
                            entity.LogisticState = State.Value;
                        }

                        if (!string.IsNullOrEmpty(LogisticWayBillNo))
                        {
                            entity.LogisticCompanyName = LogisticCompanyName;
                        }

                        if (!string.IsNullOrEmpty(LogisticWayBillNo))
                        {
                            entity.LogisticWayBillNo = LogisticWayBillNo;
                        }

                        entity.ModifyTime = DateTime.Now;
                        db.Update(entity);

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.LogisticState.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.LogisticStateChanged.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.LogisticStateChanged,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        if (db.SaveChanges() <= 0)
                        {
                            return false;
                        }
                    }
                }
                var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, entity.OrderNo);
                CacheKey_Order.RemoveCache();
                return true;
            }
        }

        /// <summary>
        /// 交易完成
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public bool Complete(string OrderNo = "", string OrderOutID = "")
        {
            Order entity = new Order();

            using (DBEntities db = new DBEntities())
            {
                if (OrderNo != "")
                {
                    entity = db.Set<Order>().Where(a => a.OrderNo == OrderNo).FirstOrDefault();
                }
                else if (OrderOutID != "")
                {
                    //只能完成用户自己的订单
                    entity = db.Set<Order>().Where(a => a.OrderOutID == OrderOutID).FirstOrDefault();
                }
                else
                {
                    return false;
                }

                if (entity != null)
                {
                    //订单已经支付
                    if (entity.OrderState == EnumOrderState.Paid)
                    {

                        entity.OrderState = EnumOrderState.Finish;
                        entity.FinishTime = DateTime.Now;
                        db.Update(entity);

                        //写订单日志
                        db.OrderLogs.Add(new OrderLog()
                        {
                            Remark = entity.OrderType.GetEnumDescript(),
                            CreateTime = DateTime.Now,
                            OperationDesc = EnumEnumOrderLogOperationType.Complete.GetEnumDescript(),
                            OperationTime = DateTime.Now,
                            OperationType = EnumEnumOrderLogOperationType.Complete,
                            OperationUserID = CurrentOperatorUserID,
                            OrderNo = entity.OrderNo,
                            OrderLogID = Guid.NewGuid().ToString("N"),
                            IsDeleted = false
                        });

                        db.TriggerEvent(new XuHos.EventBus.Events.OrderCompletedEvent()
                        {
                            OrderNo = entity.OrderNo
                        });

                        if (db.SaveChanges() > 0)
                        {
                            var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, entity.OrderNo);
                            CacheKey_Order.RemoveCache();
                            return true;
                        }

                   
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    LogHelper.WriteWarn($"订单 {OrderNo} 不存在");
                    return true;
                }

                return false;
            }
        }
        #endregion

        /// <summary>
        /// 批量刷新订单状态       
        
        /// 日期：2016年9月5日
        /// </summary>
        public void BatchRefreshStateAsync()
        {
            using (DAL.EF.DBEntities db = new DBEntities())
            {
                BLL.OrderService bllOrder = new OrderService(CurrentOperatorUserID);

                //推到24小时以前
                DateTime exprieTime24Hours = DateTime.Now.AddHours(-24);

                //推到30分钟前
                DateTime exprieTime30Minutes = DateTime.Now.AddMinutes(-30);

                #region 取消订单：未支付,超时未支付订单自动取消
                try
                {

                    var curTime = DateTime.Now;

                    var orderList = db.Orders.Where(a =>
                                                        !a.IsDeleted &&
                                                        (a.OrderState == EnumOrderState.NoPay || a.OrderState == EnumOrderState.NoConfirm) &&
                                                        a.OrderExpireTime < curTime &&
                                                        a.OrderType != EnumProductType.Consultation
                                                        && a.OrderType != EnumProductType.RenewUpgrade).ToList();

                    var failCount = 0;

                    if (orderList.Count > 0)
                        XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单超时未支付，等待取消", orderList.Count));

                    Parallel.ForEach(orderList, order =>
                    {

                        try
                        {
                            //取消订单
                            if (!bllOrder.Cancel(order.OrderNo, "超时未支付"))
                            {
                                failCount++;
                                XuHos.Common.LogHelper.WriteError(new Exception("订单:" + order.OrderNo + "取消失败"));
                            }
                        }
                        catch (Exception E)
                        {
                            failCount++;
                            XuHos.Common.LogHelper.WriteError(E);
                        }

                    });


                    if (orderList.Count > 0)
                        XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单30分钟尚未支付，取消完成。失败:{1}", orderList.Count, failCount));


                }
                catch (Exception E)
                {
                    XuHos.Common.LogHelper.WriteError(E);
                }

                #endregion

                #region 退款：已支付，重复付款的，自动退款
                try
                {
                    var query = from firstTradeLog in db.OrderTradeLogs.Where(a => !a.IsDeleted && a.TradeStatus == EnumTradeState.TRADE_SUCCESS && !string.IsNullOrEmpty(a.OnlineTransactionNo) && a.PayType != EnumPayType.OfflinePay).GroupBy(a => a.OrderNo).Where(a => a.Count() > 1).Select(a => new { OrderNo = a.Key, FirstPayTime = a.Min(b => b.CreateTime) })
                                join payOrder in db.Orders.Where(a => !a.IsDeleted) on firstTradeLog.OrderNo equals payOrder.OrderNo
                                join tradeLog in db.OrderTradeLogs.Where(a => !a.IsDeleted && a.TradeStatus == EnumTradeState.TRADE_SUCCESS) on firstTradeLog.OrderNo equals tradeLog.OrderNo
                                join refundLog in db.OrderRefundLogs.Where(a => !a.IsDeleted) on new
                                {
                                    tradeLog.OrderNo,
                                    tradeLog.PayType,
                                    tradeLog.TradeNo
                                } equals new
                                {
                                    refundLog.OrderNo,
                                    refundLog.PayType,
                                    refundLog.TradeNo
                                } into refundLogLeftJoin
                                from refundLogIfEmpty in refundLogLeftJoin.DefaultIfEmpty()
                                where (
                                        (refundLogIfEmpty == null) ||
                                        (refundLogIfEmpty.RefundState == EnumRefundState.applyRefund)
                                        ) &&
                                        tradeLog.CreateTime > firstTradeLog.FirstPayTime
                                select new
                                {
                                    tradeLog.SellerID,
                                    tradeLog.OrderNo,
                                    tradeLog.TradeNo,
                                    tradeLog.PayType,
                                    tradeLog.TradeFee,
                                    tradeLog.OnlineTransactionNo,
                                    payOrder.OrderType
                                };

                    var orderList = query.ToList();

                    var failCount = 0;

                    if (orderList.Count > 0)
                        XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单重复付款，等待处理", orderList.Count));

                    orderList.ForEach(order =>
                    {
                        try
                        {
                            var cashierService = XuHos.BLL.Platform.Cashier.CashierFactoryService.Create(order.PayType, CurrentOperatorUserID);

                            if (cashierService != null)
                            {
                                var refundLogEntity = db.Set<OrderRefundLog>().Where(a => a.OrderNo == order.OrderNo && a.PayType == order.PayType).FirstOrDefault();

                                //没有退款日志是写日志
                                if (refundLogEntity == null)
                                {
                                    refundLogEntity = new OrderRefundLog()
                                    {
                                        OrderNo = order.OrderNo,//订单好
                                        TradeNo = order.TradeNo,//交易编号
                                        PayType = order.PayType,//支付方式
                                        RefundNo = order.TradeNo,
                                        RefundFee = order.TradeFee,
                                        RefundState = EnumRefundState.Refunding,
                                        RefundTime = DateTime.Now,//交易日期       
                                        CreateTime = DateTime.Now,//创建时间
                                        IsDeleted = false
                                    };
                                    //退款日志
                                    db.OrderRefundLogs.Add(refundLogEntity);

                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(refundLogEntity.RefundNo))
                                        refundLogEntity.RefundNo = order.TradeNo;

                                    refundLogEntity.RefundState = EnumRefundState.Refunding;
                                    refundLogEntity.ModifyTime = DateTime.Now;
                                    db.Update(refundLogEntity);
                                }

                                //日志写入成功
                                if (db.SaveChanges() > 0)
                                {
                                    XuHos.Common.LogHelper.WriteDebug($"ApplyRefund:OrderNo={order.OrderNo},SellerID={order.SellerID},TradeNo={order.TradeNo},RefundNo={refundLogEntity.RefundNo},RefundFee={refundLogEntity.RefundFee}");

                                    //开始退款
                                    if (cashierService.ApplyRefund(
                                    order.OrderNo,
                                    order.SellerID,
                                    order.TradeNo,
                                    order.TradeFee,
                                    refundLogEntity.RefundNo,
                                    refundLogEntity.RefundFee,
                                    order.OnlineTransactionNo))
                                    {
                                        refundLogEntity.RefundState = EnumRefundState.Refunded;
                                        db.Update(refundLogEntity);

                                        if (db.SaveChanges() <= 0)
                                            failCount++;
                                    }
                                    else
                                    {
                                        failCount++;
                                        XuHos.Common.LogHelper.WriteWarn($"{order.PayType.GetEnumDescript()}订单 {order.OrderNo}:退款失败");
                                    }
                                }
                                else
                                {
                                    failCount++;
                                }
                            }
                            else
                            {
                                failCount++;
                                XuHos.Common.LogHelper.WriteWarn(string.Format("订单:{0} 不支持{1}类型的退款", order.OrderNo, order.PayType.GetEnumDescript()));
                            }
                        }
                        catch (Exception E)
                        {
                            failCount++;
                            XuHos.Common.LogHelper.WriteError(E);
                        }
                    });

                    if (orderList.Count > 0)
                        XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单重复付款，退款完成。失败:{1}", orderList.Count, failCount));


                }
                catch (Exception E)
                {
                    XuHos.Common.LogHelper.WriteError(E);
                }

                #endregion

                #region 退款：已申请退款，开始退款
                {
                    try
                    {
                        //已经取消，但是没有退款日志或退款日志中是申请退款或正在退款中的，需要重新退款
                        var orderList = (
                                from order in db.Orders.Where(a => !a.IsDeleted && (a.OrderState == EnumOrderState.Canceled))
                                join tradeLog in db.OrderTradeLogs.Where(a => !a.IsDeleted && !string.IsNullOrEmpty(a.OnlineTransactionNo) && (a.TradeStatus != EnumTradeState.TRADE_NOT_EXIST && a.TradeStatus != EnumTradeState.WAIT_BUYER_PAY) && a.PayType != EnumPayType.OfflinePay) on order.OrderNo equals tradeLog.OrderNo
                                join refundLog in db.OrderRefundLogs.Where(a => !a.IsDeleted) on new { tradeLog.OrderNo, tradeLog.PayType, tradeLog.TradeNo } equals new { refundLog.OrderNo, refundLog.PayType, refundLog.TradeNo } into refundLogLeftJoin
                                from refundLogIfEmpty in refundLogLeftJoin.DefaultIfEmpty()
                                where (refundLogIfEmpty == null) || (refundLogIfEmpty.RefundState == EnumRefundState.applyRefund)
                                select tradeLog
                        ).ToList();

                        var failCount = 0;

                        if (orderList.Count > 0)
                            XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单已申请退款，等待处理", orderList.Count));

                        orderList.ForEach(order =>
                       {
                           try
                           {
                               //取消订单
                               if (!bllOrder.Refund(order.OrderNo, order.PayType, order.TradeNo))
                               {
                                   failCount++;
                                   XuHos.Common.LogHelper.WriteWarn("订单:" + order.OrderNo + "申请退款失败");
                               }
                           }
                           catch (Exception E)
                           {
                               failCount++;
                               XuHos.Common.LogHelper.WriteError(E);
                           }
                       });

                        if (orderList.Count > 0)
                            XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单已申请退款，退款完成。失败:{1}", orderList.Count, failCount));


                    }
                    catch (Exception E)
                    {
                        XuHos.Common.LogHelper.WriteError(E);
                    }
                }
                #endregion

                #region 物流配送：已支付，未发货的订单自动发货(非正常支付流程的订单如：套餐、免费、义诊、会员等方式支付的)

                try
                {
                    //已支付还未发货的订单
                    var waitDeliveryOrders = db.Orders.Where(a =>
                     a.OrderState == EnumOrderState.Paid
                     && a.LogisticState == EnumLogisticState.待审核
                     && a.LogisticNo==""
                     && !a.IsDeleted).Select(a => new { a.OrderNo, a.OrderType }).ToList();

                    var failCount = 0;

                    if (waitDeliveryOrders.Count > 0)
                        XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单已支付完成，等待发货", waitDeliveryOrders.Count));


                    Parallel.ForEach(waitDeliveryOrders, order =>
                    {
                        try
                        {
                            if (!bllOrder.LogisticWithDelivery(order.OrderNo))
                            {
                                failCount++;
                                XuHos.Common.LogHelper.WriteWarn(order.OrderType.GetEnumDescript() + "订单:" + order.OrderNo + "发货失败");
                            }
                        }
                        catch (Exception E)
                        {
                            failCount++;
                            XuHos.Common.LogHelper.WriteError(E);
                        }
                    });

                    if (waitDeliveryOrders.Count > 0)
                        XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单已支付完成，发货完成。失败:{1}", waitDeliveryOrders.Count, failCount));
                }
                catch (Exception E)
                {
                    XuHos.Common.LogHelper.WriteError(E);
                }

                #endregion

                #region 物流配送：已支付,物流配送中,语音/视频/图文咨询/远程会诊 配送24小时后自动送达          
                {
                    //查询没有支付，且是24小时之前下单记录修改成取消
                    db.Orders.Where(a =>
                    !a.IsDeleted &&
                    a.OrderState == EnumOrderState.Paid
                    && (a.LogisticState == EnumLogisticState.配送中)
                    && (a.OrderType == EnumProductType.Consultation ||
                        a.OrderType == EnumProductType.Phone ||
                        a.OrderType == EnumProductType.video ||
                        a.OrderType == EnumProductType.ImageText
                        )
                    && a.OrderTime < exprieTime24Hours).Update(a => new Order()
                    {
                        OrderState = EnumOrderState.Finish,
                        LogisticState = EnumLogisticState.已送达,
                        ModifyTime = DateTime.Now,
                        FinishTime = DateTime.Now
                    });

                }
                #endregion

                #region  物流配送：已支付，物流已送达的, 语音/视频/图文咨询/家庭医生/远程会诊/用户套餐等 订单自动完成
                {
                    try
                    {
                        //查询没有支付，且是24小时之前下单记录修改成取消
                        var orderList = db.Orders.Where(a =>
                      !a.IsDeleted &&
                      a.OrderState == EnumOrderState.Paid
                      && a.LogisticState == EnumLogisticState.已送达
                      && (a.OrderType == EnumProductType.Consultation ||
                          a.OrderType == EnumProductType.Phone ||
                          a.OrderType == EnumProductType.video ||
                          a.OrderType == EnumProductType.ImageText ||
                          a.OrderType == EnumProductType.FamilyDoctor ||
                          a.OrderType == EnumProductType.UserPackage
                          )
                     ).Select(i => new { i.OrderNo, i.OrderType }).ToList();

                        var failCount = 0;
                        if (orderList.Count > 0)
                            XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单将自动完成", orderList.Count));

                        orderList.ForEach(order =>
                        {
                            try
                            {
                                if (!Complete(order.OrderNo))
                                {
                                    failCount++;
                                    XuHos.Common.LogHelper.WriteWarn(order.OrderType.GetEnumDescript() + "订单:" + order.OrderNo + "状态改为已完成失败");
                                }
                            }
                            catch (Exception E)
                            {
                                failCount++;
                                XuHos.Common.LogHelper.WriteError(E);
                            }
                        });

                        if (orderList.Count > 0)
                            XuHos.Common.LogHelper.WriteDebug(string.Format("{0}个订单自动完成。失败:{1}", orderList.Count, failCount));
                    }
                    catch (Exception E)
                    {
                        XuHos.Common.LogHelper.WriteError(E);
                    }

                }
                #endregion


            }

        }

        #region 查询

        /// <summary>
        /// 提供给BAT同步订单数据
        /// </summary>
        /// <returns></returns>
        public Response<List<OrderDTO>> QueryOrderListBAT(DateTime startTime, DateTime endTime)
        {
            startTime = DateTime.Parse(startTime.ToString("yyyy-MM-dd"));
            endTime = DateTime.Parse(endTime.AddDays(1).ToString("yyyy-MM-dd"));
            var result = new Response<List<OrderDTO>>();
            using (var db = new DBEntities())
            {
                var query = (from i in db.Orders
                             where i.IsDeleted == false && i.OrderTime >= startTime && i.OrderTime < endTime && ((int)i.OrderType != 7)
                             select new OrderDTO()
                             {
                                 OrderNo = i.OrderNo,
                                 UserID = i.UserID,
                                 TotalFee = i.totalFee,
                                 OrderState = i.OrderState,
                                 OrderType = i.OrderType,
                                 OrderTime = i.OrderTime
                             });
                result.Data = query.ToList();
            }
            return result;
        }


        /// <summary>
        /// 查询配送中的订单
        /// </summary>
        /// <returns></returns>
        public Response<List<OrderDTO>> QueryDistributioningOrderList(DateTime startTime, DateTime endTime,EnumProductType OrderType,int PageIndex,int PageSize)
        {
            var result = new Response<List<OrderDTO>>();
            using (var db = new DBEntities())
            {
                var query = (from order in db.Orders
                             where 
                             !order.IsDeleted &&
                             order.LogisticNo!="" && 
                             order.OrderState== EnumOrderState.Paid &&
                             order.LogisticState!= EnumLogisticState.已送达 && 
                             order.LogisticState != EnumLogisticState.已取消
                             && order.OrderType== OrderType
                             && order.OrderTime >= startTime &&
                             order.OrderTime < endTime
                             orderby order.ModifyTime
                             select new OrderDTO()
                             {
                                 OrderOutID=order.OrderOutID,
                                 OrderNo = order.OrderNo,
                                 LogisticNo=order.LogisticNo,
                                 LogisticState=order.LogisticState,
                                 UserID = order.UserID,
                                 TotalFee = order.totalFee,
                                 OrderState = order.OrderState,
                                 OrderType = order.OrderType,
                                 OrderTime = order.OrderTime
                             }).Skip((PageIndex-1)*PageSize).Take(PageSize);
                result.Data = query.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="trade"></param>
        /// <returns></returns>
        public List<ResponseOrderLogDTO> QueryOrderLogs(string OrderNo)
        {

            using (DBEntities db = new DBEntities())
            {
                return db.Set<OrderLog>().Where(a => a.OrderNo == OrderNo).OrderBy(a => a.CreateTime).Select(a => new ResponseOrderLogDTO()
                {
                    OperationDesc = a.OperationDesc,
                    OperationTime = a.OperationTime,
                    OperationType = a.OperationType,
                    Remark = a.Remark
                }).ToList();
            }
        }
        /// <summary>
        /// 获取未支付的订单
        
        /// 日期：2017年1月16日
        /// </summary>
        /// <returns></returns>
        public List<OrderTradeLog> QueryOrderTradeLogs(EnumPayType PayType, EnumTradeState TradeStatus, DateTime LastModifyTime, int Take = 1000)
        {
            using (DBEntities db = new DBEntities())
            {
                var query = (from tradeLog in db.OrderTradeLogs.Where(
                             a => !a.IsDeleted &&
                             a.PayType == PayType &&
                             a.TradeStatus == TradeStatus &&
                             (a.ModifyTime < LastModifyTime || !a.ModifyTime.HasValue))
                             join order in db.Orders.Where(a => !a.IsDeleted) on tradeLog.OrderNo equals order.OrderNo
                             orderby order.ModifyTime ascending, order.TradeNo
                             select tradeLog).Take(Take);
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取交易日志
        
        /// 日期：2017年6月18日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public List<OrderTradeLog> QueryOrderTradeLogs(string OrderNo)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.Set<OrderTradeLog>().Where(a => a.OrderNo == OrderNo).ToList();
            }
        }
        #endregion

    }
}