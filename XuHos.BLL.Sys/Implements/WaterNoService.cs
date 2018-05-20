using XuHos.BLL.Sys.DTOs.Response;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;

namespace XuHos.BLL.Sys.Implements
{
    public class WaterNoService
    {
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWaterNo(EnumWaterNoType type)
        {
            ICacheKey cachKey = null;
            double no = 0;
            switch (type)
            {
                case EnumWaterNoType.Identify:
                    cachKey = new StringCacheKey(StringCacheKeyType.User_Identify);
                    no = Manager.Increment(cachKey);

                    if(no < 1000)
                    {
                        if (!$"GetWaterNo_Identify".Lock($"GetWaterNo_Identify", TimeSpan.FromSeconds(10), 100, 3))
                        {
                            throw new Exception("并发冲突");
                        }

                        no = Manager.Increment(cachKey);
                        if (no < 1000)
                        {
                            using (var db = new DBEntities())
                            {
                                no = db.ConversationIMUids.Where(t => !t.IsDeleted).
                                    Select(t => t.Identifier).Max();

                                if (no < 1000) no = 1000;
                                Manager.Increment(cachKey, no);
                            }
                        }
                    }
                    return no.ToString();
                case EnumWaterNoType.ChannelID:
                    cachKey = new StringCacheKey(StringCacheKeyType.ChannelID);
                    no = Manager.Increment(cachKey);

                    if (no < 1000)
                    {
                        if (!$"GetWaterNo_Identify".Lock($"GetWaterNo_Identify", TimeSpan.FromSeconds(10), 100, 3))
                        {
                            throw new Exception("并发冲突");
                        }

                        no = Manager.Increment(cachKey);
                        if (no < 1000)
                        {
                            using (var db = new DBEntities())
                            {
                                no = db.ConversationIMUids.Where(t => !t.IsDeleted).
                                    Select(t => t.Identifier).Max();

                                if (no < 1000) no = 1000;
                                Manager.Increment(cachKey, no);
                            }
                        }
                    }
                    return no.ToString();
                default:
                    return no.ToString();
            }
        }
    }
}
