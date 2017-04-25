using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BAnalytics.MessageHandling.Entity;
using MongoDB.Driver;  
using MongoDB.Bson;  

namespace BAnalytics.MessageHandling.Mongo
{
    public class MongoHelper<T> where T : BaseEntity
    {
        private static volatile MongoHelper<T> _instance = null;
        private static readonly object _lockHelper = new object();
        protected IMongoCollection<T> Collection { get; private set; }
        private static IMongoDatabase GetDatabase(string key)
        {
            var connectString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        public static MongoHelper<T> CreateInstance()
        {
            if (_instance == null)
            {
                lock (_lockHelper)
                {
                    if (_instance == null)
                        _instance = new MongoHelper<T>();
                }
            }
            return _instance;
        }

        public MongoHelper()
        {
            var db = GetDatabase("MongoConn");
            Collection = db.GetCollection<T>(typeof (T).Name.ToLower());
        }

        public async Task InsertOneAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public void InsertOne(T entity)
        {
            Collection.InsertOne(entity);
        }

        public async Task InsertManyAsync(IEnumerable<T> entities)
        {
            await Collection.InsertManyAsync(entities);
        }
        public void InsertMany(IEnumerable<T> entities)
        {
            Collection.InsertMany(entities);
        }

        public async Task ReplaceOneAsync(T entity)
        {
            await Collection.ReplaceOneAsync(a => a.Id == entity.Id, entity);
        }

        public void ReplaceOne(T entity)
        {
            Collection.ReplaceOne(a => a.Id == entity.Id, entity);
        }

        public async Task DeleteOneAsync(T entity)
        {
            await Collection.DeleteOneAsync(a => a.Id == entity.Id);
        }

        public void DeleteOne(T entity)
        {
            Collection.DeleteOne(a => a.Id == entity.Id);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }

        public void DeleteMany(Expression<Func<T, bool>> filter)
        {
            Collection.DeleteMany(filter);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T,bool>> filter)
        {
            var t = await Collection.FindAsync(filter);
            return await t.ToListAsync();
        }

        public List<T> Find(Expression<Func<T, bool>> filter)
        {
            var t = Collection.Find(filter);
            return t.ToList();
        }

        public async Task<T> GetAsync(string id)
        {
            var t = await Collection.FindAsync(a => a.Id == id, new FindOptions<T> { Limit = 1 });
            List<T> list = await t.ToListAsync();
            T result = list.FirstOrDefault();
            return result;
        }

        public T Get(string id)
        {
            var t = Collection.Find(a => a.Id == id, new FindOptions {BatchSize = 1 });
            List<T> list = t.ToList();
            T result = list.FirstOrDefault();
            return result;
        }
    }  
}
