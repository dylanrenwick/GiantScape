namespace GiantScape.Server.Data
{
    internal class DbColumnCondition<T> : DbCondition<T>
    {
        public string ColumnName { get; set; }

        public ColumnComparisonType ComparisonType { get; set; }

        public string ComparisonTarget { get; set; }

        public DbColumnCondition(string column, ColumnComparisonType comparisonType, string target)
        {
            ColumnName = column;
            ComparisonType = comparisonType;
            ComparisonTarget = target;
        }
    }

    public enum ColumnComparisonType
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Like
    }
}
