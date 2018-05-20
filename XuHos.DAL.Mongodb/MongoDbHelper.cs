using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace XuHos.DAL.Mongodb
{
    public class MongoDbHelper
    {
        private static string connectionString = "";
        private static string database = "KMEHosp";

        public static void RegisterConfig(string _connectionString= "", string _database="KMEHosp")
        {
            database = _database;
            connectionString = _connectionString;
            
        }

        #region 新增
        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="entiry"></param>
        public static void Insert<T>(string collectionName, T entity) where T : class
        {
            if(string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            MongoClient mongo = new MongoClient(connectionString);           
            IMongoDatabase friends = mongo.GetDatabase(database);
            IMongoCollection<T> categories = friends.GetCollection<T>(collectionName);
            categories.InsertOne(entity);
            
        }

        /// <summary>
        /// 插入多个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="entiry"></param>
        public static void InsertAll<T>(string collectionName, IEnumerable<T> entity) where T : class
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }
            MongoClient mongo = new MongoClient(connectionString);
            IMongoDatabase friends = mongo.GetDatabase(database);
            IMongoCollection<T> categories = friends.GetCollection<T>(collectionName);
            categories.InsertMany(entity);
        }
        #endregion

      

        #region 查询
        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static T GetSingle<T>(string collectionName, FilterDefinition<T> query) where T : class
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }
            MongoClient mongo = new MongoClient(connectionString);
            IMongoDatabase friends = mongo.GetDatabase(database);
            IMongoCollection<T> categories = friends.GetCollection<T>(collectionName);
            return categories.Find(query).FirstOrDefault();
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static T GetSingle<T>(string collectionName, FilterDefinition<T> query, FindOptions fields) where T : class
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }
            MongoClient mongo = new MongoClient(connectionString);
            IMongoDatabase friends = mongo.GetDatabase(database);
            IMongoCollection<T> categories = friends.GetCollection<T>(collectionName);
            return categories.Find(query, fields).FirstOrDefault();
            
        }
     
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query"></param>
        /// <param name="Sort"></param>
        /// <param name="cp"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string collectionName, System.Linq.Expressions.Expression<Func<T,bool>> where, SortDefinition<T> sort, int cp, int mp,out long total) where T : class
        {
            total = 0;

            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            MongoClient mongo = new MongoClient(connectionString);         
            IMongoDatabase friends = mongo.GetDatabase(database);
            IMongoCollection<T> categories = friends.GetCollection<T>(collectionName);
            total = categories.Count(where);
            return categories.Find<T>(where).Sort(sort).Skip((cp - 1) * mp).Limit(mp).ToList();
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="entity"></param>
        public static void Delete<T>(string collectionName, FilterDefinition<T> query) where T : class
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            MongoClient mongo = new MongoClient(connectionString);        
            IMongoDatabase friends = mongo.GetDatabase(database);
            IMongoCollection<T> categories = friends.GetCollection<T>(collectionName);
            categories.DeleteOneAsync(query);
           
            
        }
        #endregion
    }
}
