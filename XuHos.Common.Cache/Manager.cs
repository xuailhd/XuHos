using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Cache
{
    /// <summary>
    /// 缓存扩展
    
    /// 日期：2016年7月29日
    /// </summary>
    public static class Manager
    {

        const string KeyPrefix = "KMEHosp:";

        static ICacheManager _instance = null;

        static Dictionary<int, ICacheManager> _instances = new Dictionary<int, ICacheManager>();

        static Func<int, ICacheManager> _getCacheManagerInstanceHandler = null;

       static int _dbNum = 0;


        static object _syncCreateInstance = new object();

        public static void Register(Func<int,ICacheManager> getCacheManagerInstance,int dbNum)
        {
            _getCacheManagerInstanceHandler = getCacheManagerInstance;
            _dbNum = dbNum;
        }
        
        /// <summary>
        /// 默认数据编号
        /// </summary>
        public static int dbNum
        {
            get
            {

                return _dbNum;
            }
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        public static ICacheManager Instance
        {
            get
            {
                //双重检查
                if (_instance == null)
                {
                    lock (_syncCreateInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = UseDb(_dbNum);
                        }
                    }
                }

                return _instance;
            }
        }

        public static ICacheManager UseDb(int dbNum)
        {
            if (_getCacheManagerInstanceHandler != null)
            {
                if (!_instances.ContainsKey(dbNum))
                {
                    _instances[dbNum] = _getCacheManagerInstanceHandler(dbNum);
                }
                return _instances[dbNum];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="cacheKey"></param>
        ///<example>
        ///
        ///</example>
        public static void RemoveCache(this Keys.ICacheKey cacheKey)
        {
            
            if (Manager.Instance != null)
            {
                Manager.Instance.RemoveCache(KeyPrefix + cacheKey.KeyName);
            }
        }

        public static T FromCache<T>(this Keys.ICacheKey cacheKey)
        {
            if (Manager.Instance != null)
            {
                return Manager.Instance.StringGet<T>(KeyPrefix + cacheKey.KeyName);
            }
            else
            {
                return default(T);
            }
        }

        public static T FromCache<T>(this Keys.EntityCacheKey<T> cacheKey)
        {
            if (Manager.Instance != null)
            {
                return Manager.Instance.StringGet<T>(KeyPrefix + cacheKey.KeyName);
            }
            else
            {
                return default(T);
            }
        }
        
        public static List<T> FromCache<T>(this Keys.EntityListCacheKey<T> cacheKey)
        {
            if (Manager.Instance != null)
            {
                return Manager.Instance.StringGet<List<T>>(KeyPrefix + cacheKey.KeyName);
            }
            else
            {
                return default(List<T>);
            }
        }
        
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="cacheKey"></param>
        /// <param name="CreateCache"></param>
        /// <returns></returns>
        public static T FromCache<T>(this Keys.ICacheKey cacheKey, Func<object, T> CreateCache,object State)
        {
            if (Manager.Instance != null)
            {
                var result = Manager.Instance.StringGet<T>(KeyPrefix + cacheKey.KeyName);

                if (result == null)
                {
                    result = CreateCache(State);

                    result.ToCache(cacheKey);
                }

                return result;
            }
            else
            {
                return default(T);
            }
        }
        
        public static void ToCache<T>(this T obj, Keys.ICacheKey cacheKey)
        {
            if (Manager.Instance != null)
            {
                Manager.Instance.StringSet(KeyPrefix + cacheKey.KeyName, obj);
            }
        }
   
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">缓存对象</param>
        /// <param name="cacheKey">键</param>
        /// <param name="cacheOutTime">过期时间</param>
        public static void ToCache<T>(this T obj, Keys.ICacheKey cacheKey, TimeSpan cacheOutTime)
        {
            if (Manager.Instance != null)
                Manager.Instance.StringSet(KeyPrefix + cacheKey.KeyName, obj, cacheOutTime);
        }

        /// <summary>
        /// 设置缓存的过期时间
        /// </summary>
        /// <param name="obj">缓存对象</param>
        /// <param name="cacheKey">键</param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheOutTime">过期时间</param>
        public static void ExpireEntryAt(this Keys.ICacheKey cacheKey, TimeSpan cacheOutTime)
        {
            if (Manager.Instance != null)
                Manager.Instance.ExpireEntryAt(KeyPrefix + cacheKey.KeyName, cacheOutTime);
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="Seconds"></param>
        /// <param name="dbNum"></param>
        public static void ExpireEntryAt(this Keys.ICacheKey cacheKey,int Seconds)
        {
            if (Manager.Instance != null)
                Manager.Instance.ExpireEntryAt(KeyPrefix + cacheKey.KeyName,DateTime.Now.AddSeconds(Seconds)- DateTime.Now);
        }


        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dbNum"></param>
        /// <returns></returns>
        public static double Increment(this Keys.StringCacheKey cacheKey)
        {
            if (Manager.Instance != null)
                return Manager.Instance.StringIncrement(KeyPrefix + cacheKey.KeyName);
            else
                return default(double);
        }

        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dbNum"></param>
        /// <returns></returns>
        public static double Decrement(this Keys.StringCacheKey cacheKey)
        {
            if (Manager.Instance != null)
                return Manager.Instance.StringDecrement(KeyPrefix + cacheKey.KeyName);
            else
                return default(double);
        }

        /// <summary>
        /// 获取分布式锁
        
        /// 日期：2017年9月17日
        /// </summary>
        /// <param name="lockName">锁名称</param>
        /// <param name="LockOutTime">过期时间</param>
        /// <param name="retrySplitMillseconds">自旋锁重试间隔时间（默认50毫秒）</param>
        /// <param name="retryTimes">自旋重试次数(默认10此)</param>
        /// <returns></returns>
        public static bool Lock(
            this string lockName,
            string lockValue,
            TimeSpan LockOutTime,
            int retrySplitMillseconds=50,
            int retryTimes=5)
        {
            if (Manager.Instance != null)
            {
                var cacheKey = KeyPrefix + "Lock:" + lockName;

                do
                {
                    if (!Manager.Instance.LockTake(cacheKey, lockValue, LockOutTime))
                    {
                        retryTimes--;
                        if (retryTimes < 0)
                        {
                            return false;
                        }

                        Console.WriteLine($"Wait Lock {lockName} to {retrySplitMillseconds} millseconds");

                        //获取锁失败则进行锁等待
                        System.Threading.Thread.Sleep(retrySplitMillseconds);
                    }
                    else
                    {
                        return true;
                    }
                }
                while (retryTimes>0);
            }

            //获取锁超时返回
            return false;
            
        }


        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <param name="lockName"></param>
        /// <returns></returns>
        public static bool UnLock(this string lockName,string lockValue)
        {           
            if (Manager.Instance != null)
            {
                return Manager.Instance.LockRelease(KeyPrefix + "Lock:" + lockName, lockValue);
            }
            else
            {
                return false;
            }            
        }

    }
}
