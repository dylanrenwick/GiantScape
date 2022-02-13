namespace GiantScape.Server.Data
{
    internal class DbCondition<T>
    {
        public static DbColumnCondition<T> Column(
            string columnName,
            ColumnComparisonType comparisonType,
            string comparisonTarget)
        {
            return new DbColumnCondition<T>(columnName, comparisonType, comparisonTarget);
        }

        public static DbColumnCondition<T> ColumnEquals(string columnName, string comparisonTarget)
        {
            return new DbColumnCondition<T>(columnName, ColumnComparisonType.Equals, comparisonTarget);
        }
    }
}
