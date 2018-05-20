using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

using System.Configuration;
using KMEHosp.Common.Config;
namespace KMEHosp.Common.Cache.Redis.ServiceStackImplement
{


    public class RedisCache : ICacheManager
    {

        #region private
        private static RedisCache instance;
        private static object lock_obj = new Object();
        //虚拟节点数量
        private static readonly int VIRTUAL_NODE_COUNT = 1024;
        //Redis集群分片存储定位器
        private static KMEHosp.Common.KetamaHash.KetamaNodeLocator Locator;

        //是否启用了哨兵
        private static bool enableSentinel = false;

        private static string KeyPrefix = "";

        //链接池管理对象
        private static Dictionary<string, IRedisClientsManager> pooledClientManagers = new Dictionary<string, IRedisClientsManager>();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private RedisCache()
        {

        }

        /// <summary>
        /// 分割服务器列表
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        private static IEnumerable<string> SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        public static List<string> GetSlaveServerList(string readServerList, string clusterName)
        {

            var array = SplitString(readServerList, ",").ToList();
            List<string> result = new List<string>();
            for (int i = 0; i < array.Count; i++)
            {

                var schme = SplitString(array[i], "@").ToList();

                if (schme[0] == clusterName)
                {
                    result.Add(schme[1]);
                }
            }

            return result;
        }

        public static string GetServerClusterName(string value)
        {
            var list = SplitString(value, "@").ToList();
            if (list.Count == 2)
            {
                return list[0];
            }
            else
            {
                return "";
            }
        }

        public static string GetServerHost(string value)
        {
            var list = SplitString(value, "@").ToList();
            if (list.Count == 2)
            {
                return list[1];
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        public static RedisCache Create(KMEHosp.Common.Config.Sections.Redis config)
        {

            KeyPrefix = config.KeyPrefix + "/";

            if (true || string.IsNullOrEmpty(config.SentineList))
            {
                enableSentinel = false;

                //Redis服务器相关配置
                string writeServerList = config.WriteServerList;
                string readServerList = config.ReadServerList;
                string maxWritePoolSize = config.MaxWritePoolSize;
                string maxReadPoolSize = config.MaxReadPoolSize;
                string autoStart = config.AutoStart;
                autoStart = string.IsNullOrEmpty(autoStart) ? "true" : autoStart;
                var writeServerArray = SplitString(writeServerList, ",").ToList();
                var readServerArray = SplitString(readServerList, ",").ToList();

                //启用多个Master/多个Slave模式
                /*
                 * Redis.ReadServerList	master-6378@192.168.100.51:16378,master-6379@192.168.100.51:16379,master-6380@192.168.100.51:16380,master-6381@192.168.100.51:16381,master-6382@192.168.100.51:16382,master-6378@192.168.100.51:26378,master-6379@192.168.100.51:26379,master-6380@192.168.100.51:26380,master-6381@192.168.100.51:26381,master-6382@192.168.100.51:26382
                   Redis.WriteServerList	master-6378@192.168.100.51:6378,master-6379@192.168.100.51:6379,master-6380@192.168.100.51:6380,master-6381@192.168.100.51:6381,master-6382@192.168.100.51:6382
                 */

                for (int i = 0; i < writeServerArray.Count; i++)
                {
                    if (writeServerArray[i].IndexOf("@") > 0)
                    {
                        var clusterName = GetServerClusterName(writeServerArray[i]);
                        var masterServer = GetServerHost(writeServerArray[i]);
                        var slaveServerArray = GetSlaveServerList(config.ReadServerList, clusterName);

                        if (!pooledClientManagers.ContainsKey(writeServerArray[i]))
                        {
                            var client = new PooledRedisClientManager(
                                      new List<string> { masterServer },
                                      slaveServerArray,
                                      new RedisClientManagerConfig
                                      {
                                          MaxWritePoolSize = int.Parse(maxWritePoolSize),
                                          MaxReadPoolSize = int.Parse(maxReadPoolSize),
                                          AutoStart = Convert.ToBoolean(autoStart),
                                      });

                            pooledClientManagers.Add(writeServerArray[i], client);
                        }
                    }
                    else
                    {
                        if (!pooledClientManagers.ContainsKey(writeServerArray[i]))
                        {
                            var client = new PooledRedisClientManager(writeServerArray[i]);
                            pooledClientManagers.Add(writeServerArray[i], client);
                        }
                    }
                }

                Locator = new KMEHosp.Common.KetamaHash.KetamaNodeLocator(writeServerArray, VIRTUAL_NODE_COUNT);

            }
            else
            {
                /*
                 enableSentinel = true;

                 List<string> sentinelMasterNameList = new List<string>();
                 List<string> sentinelServerHostList = new List<string>();
                 var SentineList = SplitString(config.SentineList, ",").ToList();
                 for (int i = 0; i < SentineList.Count; i++)
                 {
                     var args = SplitString(SentineList[i], "@").ToList();
                     sentinelMasterNameList.Add(args[0]);
                     sentinelServerHostList.Add(args[1]);
                 }


                 for (int i = 0; i < sentinelServerHostList.Count; i++)
                 {
                     RedisSentinel sentinel = new RedisSentinel(sentinelServerHostList[i], sentinelMasterNameList[i]);
                     pooledClientManagers.Add(sentinelServerHostList[i], sentinel.Start());
                 }

                 //初始化Reds分片定位器
                 Locator = new KMEHosp.Common.KetamaHash.KetamaNodeLocator(sentinelServerHostList, VIRTUAL_NODE_COUNT);
                 */
            }



            return Instance;
        }

        #endregion

        /// <summary>
        /// 创建实例
        /// </summary>
        public static RedisCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lock_obj)
                    {
                        if (instance == null)
                            instance = new RedisCache();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 根据缓存名称定位需要访问的缓存服务器
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        IRedisClientsManager GetPooledClientManager(string cacheKey)
        {
            var nodeName = Locator.GetPrimary(cacheKey);


            var PooledClientManager = pooledClientManagers[nodeName];
            return PooledClientManager;
        }



        /// <summary>
        /// 获取缓存--create by guochao
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey)
        {
            T cacheData = default(T);
            if (!string.IsNullOrEmpty(cacheKey))
            {
                using (IRedisClient redisClient = GetPooledClientManager(KeyPrefix + cacheKey).GetReadOnlyClient())
                {
                    cacheData = redisClient.Get<T>(KeyPrefix + cacheKey);
                }
            }
            return cacheData;
        }

        /// <summary>
        /// 设置缓存--create by guochao
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        public bool SetCache<T>(string cacheKey, T cacheValue)
        {
            if (!string.IsNullOrEmpty(cacheKey) && cacheValue != null)
            {
                using (IRedisClient redisClient = GetPooledClientManager(KeyPrefix + cacheKey).GetClient())
                {
                    return redisClient.Set<T>(KeyPrefix + cacheKey, cacheValue);
                }
            }

            return false;
        }

        /// <summary>
        /// 设置缓存，可以加缓存过期时间--create by guochao
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="cacheOutTime"></param>
        public bool SetCache<T>(string cacheKey, T cacheValue, DateTime cacheOutTime)
        {
            RemoveCache(cacheKey);
            if (!string.IsNullOrEmpty(cacheKey) && cacheValue != null)
            {
                if (cacheOutTime != null)
                {
                    using (IRedisClient redisClient = GetPooledClientManager(KeyPrefix + cacheKey).GetClient())
                    {
                        return redisClient.Set<T>(KeyPrefix + cacheKey, cacheValue, cacheOutTime);
                    }
                }
                else
                {
                    return SetCache<T>(KeyPrefix + cacheKey, cacheValue);
                }
            }

            return false;
        }

        /// <summary>
        /// 移除缓存--create by guochao
        /// </summary>
        /// <param name="cacheKey"></param>
        public bool RemoveCache(string cacheKey)
        {
            using (IRedisClient redisClient = GetPooledClientManager(KeyPrefix + cacheKey).GetClient())
            {
                return redisClient.Remove(KeyPrefix + cacheKey);
            }
        }

        /// <summary>
        /// 设置缓存的过期时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheOutTime"></param>
        public bool ExpireEntryAt(string cacheKey, DateTime cacheOutTime)
        {
            using (IRedisClient redisClient = GetPooledClientManager(KeyPrefix + cacheKey).GetClient())
            {
                return redisClient.ExpireEntryAt(KeyPrefix + cacheKey, cacheOutTime);
            }
        }

    }
}
