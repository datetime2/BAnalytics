using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BAnalytics.MessageHandling.Entity;
using BAnalytics.MessageHandling.Mongo;

namespace BAnalytics.MessageHandling.Dao
{
    public abstract class BaseDao<T> where T : BaseEntity
    {
        public MongoHelper<T> MongoHelper;
        protected BaseDao()
        {
            MongoHelper = MongoHelper<T>.CreateInstance();
        }

        public void GetAndSave(string id, Action insertHandle, Action<T> replaceHandle)
        {
            T e = MongoHelper.Get(id);
            if (e == null)
            {
                insertHandle();
            }
            else
            {
                replaceHandle(e);
            }
        }
    }
}
