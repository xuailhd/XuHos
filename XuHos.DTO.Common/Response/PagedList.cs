using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public class PagedList<T> : List<T>
    {
        public PagedList() { }

        public PagedList(IQueryable<T> source, int? index, int? pageSize)
        {
            if (index == null) { index = 1; }
            if (pageSize == null) { pageSize = 10; }
            this.TotalCount = source.Count();
            this.PageSize = pageSize.Value;
            this.PageIndex = index.Value;
            this.AddRange(source.Skip((index.Value - 1) * pageSize.Value).Take(pageSize.Value));
        }

        public PagedList(IEnumerable<T> source, int? index, int? pageSize)
        {
            if (index == null) { index = 1; }
            if (pageSize == null) { pageSize = 10; }
            this.TotalCount = source.Count();
            this.PageSize = pageSize.Value;
            this.PageIndex = index.Value;
            this.AddRange(source.Skip((index.Value - 1) * pageSize.Value).Take(pageSize.Value));
        }

        public PagedList(IEnumerable<T> source, int? index, int? pageSize, int? total)
        {
            if (index == null) { index = 1; }
            if (pageSize == null) { pageSize = 10; }
            this.TotalCount = total.Value;
            this.PageSize = pageSize.Value;
            this.PageIndex = index.Value;
            this.AddRange(source.Skip((index.Value - 1) * pageSize.Value).Take(pageSize.Value));
        }

        public int TotalPage
        {
            get { return (int)System.Math.Ceiling((double)TotalCount / PageSize); }
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool IsNextPage
        {
            get
            {
                return ((PageIndex) * PageSize) < TotalCount;
            }
        }
    }

    public static class Pagination
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int? index, int? pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int? index)
        {
            return new PagedList<T>(source, index, 10);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int? index, int? pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int? index, int? pageSize, int? total)
        {
            return new PagedList<T>(source, index, pageSize, total);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int? index)
        {
            return new PagedList<T>(source, index, 10);
        }
    }
}
