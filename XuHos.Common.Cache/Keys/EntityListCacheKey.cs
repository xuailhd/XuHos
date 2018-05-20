using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Cache.Keys
{
    public class EntityListCacheKey<T> : ICacheKey
    {
        string _KeyId        { get; set; }
        string _KeyType { get; set; }


        public EntityListCacheKey(StringCacheKeyType KeyType, string KeyId)
        {
            this._KeyType = KeyType.ToString();
            this._KeyId = KeyId;
        }

        public EntityListCacheKey()
        {
            this._KeyId = "";
        }

        public string KeyName
        {
            get
            {
                if (string.IsNullOrEmpty(_KeyId))
                    return $"{_KeyType}";
                else
                    return $"{_KeyType}:{_KeyId}";

            }

        }

    }
}
