using Microsoft.EntityFrameworkCore;
using LemonExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LemonExam.Infrastructure {
    public static class DALExtensions {

        private static object nextIdLock = new object();
        private static Dictionary<Type, int> IDs = new Dictionary<Type, int>();

        public static int NextID<T>(this DbSet<T> dbSet) where T : BaseEntity {
            if (dbSet == null) {
                throw new ArgumentNullException("dbSet");
            }
            lock (nextIdLock) {
                int result = 0;
                var type = typeof(T);
                if (IDs.ContainsKey(type))
                    result = ++IDs[type];
                else {
                    result = dbSet.Max(m => (int?)m.ID) ?? 0;
                    IDs.Add(type, ++result);
                }

                return result;
            }
        }
    }
}
