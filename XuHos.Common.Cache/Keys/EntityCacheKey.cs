using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Cache.Keys
{
    public class EntityCacheKey<T>:ICacheKey
    {
        string _ID;
        string _KeyType { get; set; }

        public EntityCacheKey(StringCacheKeyType KeyType, string ID)
        {
            this._KeyType = KeyType.ToString();
            this._ID = ID;
        }

        public string KeyName
        {
            get
            {
                return $"{_KeyType}:{_ID}";
            }
        }
    }
}
