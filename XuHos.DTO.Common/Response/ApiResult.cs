using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XuHos.Extensions;
using XuHos.Common.Enum;

namespace XuHos.DTO.Common
{
    public static class ApiResultExtend
    {
        private static readonly object TRUE = true;

        private static readonly object FALSE = false;

        /// <summary>
        /// 返回API标准结果（根据布尔值决定返回成功还是失败）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ApiResult ToApiResultForBoolean(this bool obj, string Msg = "")
        {
            var Status = obj ? EnumApiStatus.BizOK : EnumApiStatus.BizError;

            return new ApiResult
            {
                Data = obj ? TRUE : FALSE,
                Msg = Msg == "" ? Status.GetEnumDescript() : Msg,
                Status = Status,
                Result = true
            };
        }

        public static ApiResult ToApiResultForApiStatus(this EnumApiStatus obj, string Msg = "")
        {
            return new ApiResult
            {
                Msg = string.IsNullOrEmpty(Msg) ? obj.GetEnumDescript() : Msg,
                Status = obj,
                Result = true,
            };
        }

        public static ApiResult ToApiResultForApiStatus(this EnumApiStatus obj, object Data, string Msg = "")
        {
            return new ApiResult
            {
                Data = Data,
                Msg = string.IsNullOrEmpty(Msg) ? obj.GetEnumDescript() : Msg,
                Status = obj,
                Result = true
            };
        }

        /// <summary>
        /// 根据ApiStatus的类型转换Data为true或者False
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ApiResult AsStatusToBoolean(this ApiResult obj)
        {
            if (obj.Status == EnumApiStatus.BizOK)
                obj.Data = TRUE;
            else
                obj.Data = FALSE;

            return obj;
        }

        public static ApiResult ToApiResultForObject(this object obj, EnumApiStatus Status = EnumApiStatus.BizOK, string Msg = "")
        {
            return new ApiResult
            {
                Data = obj,
                Msg = string.IsNullOrEmpty(Msg) ? Status.GetEnumDescript() : Msg,
                Status = Status,
                Result = true,
            };
        }


        /// <summary>
        /// 将列表转成Api标准结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        /// <param name="Msg"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static ApiResult ToApiResultForList<TEntity>(this IResponse<TEntity> obj, EnumApiStatus Status = EnumApiStatus.BizOK, string Msg = "")
        {
            return new ApiResult
            {
                Data = obj.Data,
                Total = obj.Total,
                Msg = string.IsNullOrEmpty(Msg) ? Status.GetEnumDescript() : Msg,
                Status = Status,
                Result = true,
            };
        }
    }

    /// <summary>
    ///  API返回单个实体类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult : ApiMessageResult
    {
        public ApiResult()
            : base(0, "操作成功")
        {
        }

        public ApiResult(object data)
            : base(0, "操作成功")
        {
            this.Data = data;
        }

        public ApiResult(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public int Total { get; set; }
    }

    /// <summary>
    /// 接收API的结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public new T Data
        {
            get { return base.Data != null && base.Data is T ? (T)base.Data : default(T); }
            set { base.Data = value; }
        }
    }

    /// <summary>
    /// API返回消息基类
    /// </summary>
    public class ApiMessageResult
    {
        public ApiMessageResult()
        {
        }

        public ApiMessageResult(EnumApiStatus status, string msg)
        {
            this.Status = status;
            this.Msg = msg;
        }

        public ApiMessageResult(Exception ex)
        {
            this.Status = EnumApiStatus.BizError;
            this.Msg = "操作失败：" + ex.GetDetailException();
        }

        /// <summary>
        /// 接口业务状态
        /// </summary>
        public EnumApiStatus Status { get; set; }

        /// <summary>
        /// 消息状态说明
        /// </summary>
        public string Msg { get; set; }


        /// <summary>
        /// 接口是否调用成功
        /// </summary>
        public bool Result { get; set; }
    }
}