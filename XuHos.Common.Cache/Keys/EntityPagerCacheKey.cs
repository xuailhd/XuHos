using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Cache.Keys
{
    public class EntityPagerCacheKey<T> : ICacheKey
    {
        int _PageSize = 0;
        int _PageIndex = 0;
        string _KeyType { get; set; }
        public EntityPagerCacheKey(StringCacheKeyType KeyType, int PageSize, int PageIndex)
        {
            this._KeyType = KeyType.ToString();
            this._PageSize = PageSize;
            this._PageIndex = PageIndex;
        }

        public string KeyName
        {
            get
            {
                return $"{_KeyType}:size:{_PageSize}:index:{_PageIndex}";
            }

        }

    }
}
