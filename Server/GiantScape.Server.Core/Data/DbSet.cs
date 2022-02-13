using System.Collections.Generic;

namespace GiantScape.Server.Data
{
    internal abstract class DbSet<T>
    {
        public abstract DbQuery<T> Where(DbCondition<T> condition);

        public abstract IEnumerable<T> Query(DbCondition<T> condition);
    }
}
