using System;
using System.Collections.Generic;

namespace GiantScape.Server.Data.SQLite
{
    internal class SqliteDbQuery<T> : DbQuery<T>
    {
        public SqliteDbQuery(DbSet<T> set, DbCondition<T> condition)
            : base(set, condition)
        {
        }
    }
}
