using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Cache.Keys
{


    public class StringCacheKey:ICacheKey
    {
        string _KeyId { get; set; }

        string _KeyType { get; set; }


        public StringCacheKey(StringCacheKeyType KeyType, string KeyId)
        {
            this._KeyType = KeyType.ToString();
            this._KeyId = KeyId;
        }
        public StringCacheKey(StringCacheKeyType KeyType)
        {
            this._KeyType = KeyType.ToString();
        }
        public string KeyName
        {
            get
            {
                return $"{_KeyType}:{_KeyId}";
            }
        }
    }
}
