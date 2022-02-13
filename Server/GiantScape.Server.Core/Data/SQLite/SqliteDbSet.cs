using System.Text;
using System.Collections.Generic;

using Microsoft.Data.Sqlite;

namespace GiantScape.Server.Data.SQLite
{
    internal class SqliteDbSet<T> : DbSet<T>
    {
        private readonly SqliteConnection connection;

        public SqliteDbSet(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public override DbQuery<T> Where(DbCondition<T> condition)
        {
            return new SqliteDbQuery<T>(this, condition);
        }

        public override IEnumerable<T> Query(DbCondition<T> condition)
        {
            string conditionWhereClause = EvaluateCondition(condition);

        }

        private static string EvaluateCondition(DbCondition<T> condition)
        {
            StringBuilder sb = new StringBuilder();
            EvaluateCondition(sb, condition);
            return sb.ToString();
        }
        private static void EvaluateCondition(StringBuilder sb, DbCondition<T> condition)
        {
            if (condition is DbCompoundCondition<T> compoundCondition)
            {
                EvaluateCondition(sb, compoundCondition.FirstCondition);
                switch (compoundCondition.ConditionType)
                {
                    case CompoundConditionType.Or:  sb.Append(" OR ");  break;
                    case CompoundConditionType.And: sb.Append(" AND "); break;
                }
                EvaluateCondition(sb, compoundCondition.SecondCondition);
            }
            else if (condition is DbColumnCondition<T> columnCondition)
            {
                sb.Append(columnCondition.ColumnName);
                switch (columnCondition.ComparisonType)
                {
                    case ColumnComparisonType.Equals: sb.Append(" = "); break;
                    case ColumnComparisonType.NotEquals: sb.Append(" <> "); break;
                    case ColumnComparisonType.GreaterThan: sb.Append(" > "); break;
                    case ColumnComparisonType.LessThan: sb.Append(" < "); break;
                    case ColumnComparisonType.GreaterThanOrEqual: sb.Append(" >= "); break;
                    case ColumnComparisonType.LessThanOrEqual: sb.Append(" <= "); break;
                    case ColumnComparisonType.Like: sb.Append(" LIKE "); break;
                    default: break;
                }
                sb.Append(columnCondition.ComparisonTarget);
            }
        }
    }
}
