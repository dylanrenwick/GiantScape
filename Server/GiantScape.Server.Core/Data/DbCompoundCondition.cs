namespace GiantScape.Server.Data
{
    internal class DbCompoundCondition<T> : DbCondition<T>
    {
        public CompoundConditionType ConditionType { get; set; }

        public DbCondition<T> FirstCondition { get; set; }
        public DbCondition<T> SecondCondition { get; set; }

    }
    public enum CompoundConditionType
    {
        And,
        Or
    }
}
