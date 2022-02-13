using System.Collections.Generic;

namespace GiantScape.Server.Data
{
    internal abstract class DbQuery<T>
    {
        public DbSet<T> Set { get; private set; }

        public DbCondition<T> Condition { get; set; }

        public DbQuery(DbSet<T> set, DbCondition<T> condition)
        {
            Set = set;
            Condition = condition;
        }

        public virtual IEnumerable<T> Evaluate()
        {
            return Set.Query(Condition);
        }
    }
}
