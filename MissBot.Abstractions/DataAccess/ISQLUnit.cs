namespace MissBot.Abstractions.DataAccess
{
    public interface ISQL<in TUnit> : ISQLUnit
    {
        TResult GetResponse<TResult>(ISQLQuery<TUnit>query, CancellationToken cancel = default) where TResult:TUnit ;
    }

    public interface ISQLQuery<out TResult> : ISQLUnit
    {
        TResult Response { get; }
    }

    public interface ISQLUnit
    {
        //ISQLResponse<TResult> GetResponse<TResult>(TResult result = default);
        //void SetResult<TResult>(TResult result = default, int errorCode = 0, string description = default);
        SQLResult Result { get; set; }
        SQLCommand Command { get; }
    }
}
