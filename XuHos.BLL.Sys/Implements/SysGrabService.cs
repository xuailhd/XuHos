using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common;
using XuHos.Common.Cache;
namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 系统抢单服务
    
    /// 日期：2017年5月16日
    /// </summary>
    public class SysGrabService<T>
    {
        private XuHos.Common.Cache.Keys.EntityCacheKey<T> _CacheKey_TaskPool = null;
        private XuHos.Common.Cache.Keys.EntityCacheKey<T> _CacheKey_LatestTakeTime = null;
        private const int min_priority = 1;
        private const int max_priority = 5;
        private string _name;
        private string _CacheKey_DerepList = "";
    	private string _Scope
        {
            get { return DateTime.Now.ToString("yyyyMMdd"); }
        }

        public SysGrabService(string name)
        {
            this._name = name;          

            _CacheKey_TaskPool = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, name);
            _CacheKey_LatestTakeTime = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:LatestTakeTime");
            _CacheKey_DerepList = $"{_CacheKey_TaskPool.KeyName}:Dereplication";
        }

        /// <summary>
        /// 领取任务    
        /// 从主队列移除后放入临时队列，此操作是原子操作
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public XuHos.Common.Enum.EnumApiStatus TakeTask(string category, out T task, List<string> taskGroups)
        {
            task = default(T);

            if (taskGroups == null)
            {
                taskGroups = new List<string>() { "ALL" };
            }
            else
            {
                taskGroups.Add("ALL");
            }

            if (Manager.Instance != null)
            {
                // 记录最后一次领单时间
                Manager.Instance.StringSet<DateTime?>(_CacheKey_LatestTakeTime.KeyName, DateTime.Now);

                var CacheKey_Waiting = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Waiting");

                //按照分组下优先级获取
                Func<int, string, T> getGroupPriorityTask = (p, taskGroupName) =>
                {
                    return Manager.Instance.ListRightPopLeftPush<T>($"{_CacheKey_TaskPool.KeyName}:{taskGroupName}:{p}", CacheKey_Waiting.KeyName);
                };

                //当前任务已经被消耗了才重新获取新任务 
                if (Manager.Instance.ListRange<T>(CacheKey_Waiting.KeyName).Count <= 0)
                {
                    if (task == null)
                    {
                        foreach (var taskGroupName in taskGroups)
                        {
                            //依次获取
                            for (int i = max_priority; i >= min_priority; i--)
                            {
                                task = getGroupPriorityTask(i, taskGroupName);

                                if (task != null)
                                {
                                    return XuHos.Common.Enum.EnumApiStatus.BizOK;
                                }
                            }
                        }
                    }
                }
                else
                {
                    task = Manager.Instance.ListRange<T>(CacheKey_Waiting.KeyName).FirstOrDefault();

                    return XuHos.Common.Enum.EnumApiStatus.BizDoctorTaskAlreadyTaskUnhandledFinish;
                }

                return XuHos.Common.Enum.EnumApiStatus.BizDoctorTaskPoolEmpty;
            }
            else
            {
                return XuHos.Common.Enum.EnumApiStatus.BizError;
            }
        }

        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <param name="item"></param>
        public void StartTask(T item, string category)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_Waiting = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Waiting");
                var CacheKey_Doing = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Doing");

                //删除重复列表记录
                Manager.Instance.SetRemove(_CacheKey_DerepList, item);
                Manager.Instance.ListRemove<T>(CacheKey_Waiting.KeyName, item);
                Manager.Instance.ListLeftPush(CacheKey_Doing.KeyName, item);                
            }
        }

    

        /// <summary>
        /// 领取任务后，执行完成
        /// </summary>
        /// <param name="item"></param>
        public void FinishTask(T item, string category)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_DoingTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Doing");
                var CacheKey_Finished = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Finished");

                Manager.Instance.ListRemove<T>(CacheKey_DoingTask.KeyName, item);
                Manager.Instance.ListLeftPush(CacheKey_Finished.KeyName, item);
            }
        }

        /// <summary>
        /// 领取任务后拒绝执行
        /// </summary>
        /// <param name="item"></param>
        public void RejectTask(T item, string category, int priority)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_DoingTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Doing");
                var CacheKey_WaitTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Waiting");

                Manager.Instance.ListRemove<T>(CacheKey_DoingTask.KeyName, item);
                Manager.Instance.ListRemove<T>(CacheKey_WaitTask.KeyName, item);
                Manager.Instance.ListLeftPush<T>($"{_CacheKey_TaskPool.KeyName}:{priority}", item);
            }
        }

        /// <summary>
        /// 派遣任务
        /// </summary>
        /// <param name="item"></param>
        public void DispatchTask(T item, int priority, string taskGroupID = "ALL")
        {
            // >5  || <1
            if (priority < min_priority || priority > max_priority)
            {
                priority = min_priority;
            }


            if (Manager.Instance != null)
            {
                //不重复才写入
                if (!Manager.Instance.SetContains(_CacheKey_DerepList, item))
                {
                    //写去重复列表
                    Manager.Instance.SetAdd(_CacheKey_DerepList, item);

                    Manager.Instance.ListLeftPush<T>($"{_CacheKey_TaskPool.KeyName}:{taskGroupID}:{priority}", item);

                    // 记录最后一次领单时间
                    if (!this.LatestTakeTime.HasValue)
                        Manager.Instance.StringSet<DateTime?>(_CacheKey_LatestTakeTime.KeyName, DateTime.Now);

                }
            }

        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="item"></param>
        public void CancelTask(T item, string taskGroupID = "ALL")
        {
            Action<int> cancelTaskHandler = (p) =>
            {
                Manager.Instance.ListRemove<T>($"{_CacheKey_TaskPool.KeyName}:{taskGroupID}:{p}", item);
            };

            //删除重复列表记录
            Manager.Instance.SetRemove(_CacheKey_DerepList, item);

            //依次获取
            for (int i = max_priority; i >= min_priority; i--)
            {
                cancelTaskHandler(i);
            }
        }

        public void InvalidTask(T item, string category)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_Waiting = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Waiting");
                Manager.Instance.ListRemove<T>(CacheKey_Waiting.KeyName, item);
            }
        }

        #region TaskPool
        /// <summary>
        /// 任务列表是空的
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, System.Collections.Generic.Dictionary<int, bool>> TaskListEmpty(List<string> taskGroups)
        {
            if (taskGroups == null)
            {
                taskGroups = new List<string>() { "ALL" };
            }
            else
            {
                taskGroups.Add("ALL");
            }

            Dictionary<string, System.Collections.Generic.Dictionary<int, bool>> result = new Dictionary<string, Dictionary<int, bool>>();

            if (Manager.Instance != null)
            {
                foreach (var taskGroupName in taskGroups)
                {
                    if (!result.ContainsKey(taskGroupName))
                        result[taskGroupName] = new Dictionary<int, bool>();

                    //依次获取
                    for (int i = max_priority; i >= min_priority; i--)
                    {
                        result[taskGroupName].Add(i, Manager.Instance.ListLength($"{_CacheKey_TaskPool.KeyName}:{taskGroupName}:{i}") <= 0);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 任务总数量
        /// </summary>
        public Dictionary<string, System.Collections.Generic.Dictionary<int, long>> TaskCount(List<string> taskGroups = null)
        {
            if (taskGroups == null)
            {
                taskGroups = new List<string>() { "ALL" };
            }
            else
            {
                taskGroups.Add("ALL");
            }

            Dictionary<string, System.Collections.Generic.Dictionary<int, long>> result = new Dictionary<string, Dictionary<int, long>>();


            if (Manager.Instance != null)
            {
                foreach (var taskGroupName in taskGroups)
                {
                    if (!result.ContainsKey(taskGroupName))
                        result[taskGroupName] = new Dictionary<int, long>();

                    //依次获取
                    for (int i = max_priority; i >= min_priority; i--)
                    {
                        if (!result[taskGroupName].ContainsKey(i))
                        {
                            result[taskGroupName].Add(i, Manager.Instance.ListLength($"{_CacheKey_TaskPool.KeyName}:{taskGroupName}:{i}"));
                        }
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// 任务总数量合计
        
        /// 日期：2017年8月17日
        /// </summary>
        public long TaskSum(List<string> taskGroups = null)
        {
            var result = TaskCount(taskGroups);

            return result.Sum(a => a.Value.Sum(b => b.Value));
        }

        public long TaskCount(int priority, List<string> taskGroups)
        {
            if (taskGroups == null)
            {
                taskGroups = new List<string>() { "ALL" };
            }
            else
            {
                taskGroups.Add("ALL");
            }

            if (Manager.Instance != null)
            {
                foreach (var taskGroupName in taskGroups)
                {
                    return Manager.Instance.ListLength($"{_CacheKey_TaskPool.KeyName}:{taskGroupName}:{priority}");
                }
            }

            return 0;
        }

        #endregion

        #region Doing
        /// <summary>
        /// 领取的任务列表是空的
        /// </summary>
        /// <returns></returns>
        public bool DoingTaskListEmpty(string category)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_DoingTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Doing");
                return Manager.Instance.ListLength(CacheKey_DoingTask.KeyName) <= 0;


            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 已领取的任务数量
        /// </summary>
        public long DoingTaskCount(string category)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_DoingTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Doing");
                return Manager.Instance.ListLength(CacheKey_DoingTask.KeyName);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Finished

        /// <summary>
        /// 已完成的人数列表是否为空
        /// </summary>
        /// <returns></returns>
        public bool FinishedTaskListEmpty(string category)
        {
            if (Manager.Instance != null)
            {
                var CacheKey_FinishedTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Finished");
                return Manager.Instance.ListLength(CacheKey_FinishedTask.KeyName) <= 0;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 已完成的任务数量
        /// </summary>
        public long FinishedTaskCount(string category)
        {
            
            if (Manager.Instance != null)
            {
                var CacheKey_FinishedTask = new XuHos.Common.Cache.Keys.EntityCacheKey<T>(XuHos.Common.Cache.Keys.StringCacheKeyType.Grab, $"{_name}:{_Scope}:{category}:Finished");
                return Manager.Instance.ListLength(CacheKey_FinishedTask.KeyName);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// 最后一次领单时间
        /// </summary>
        public DateTime? LatestTakeTime
        {
            get
            {
                return Manager.Instance.StringGet<DateTime?>(_CacheKey_LatestTakeTime.KeyName);
            }
        }

    }
}
