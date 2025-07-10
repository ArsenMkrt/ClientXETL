namespace ClientXETL.Services.SearchIndexes;

public class SearchIndex<TIndex, TType>(IReadOnlyCollection<TType> data, Func<TType, TIndex> keySelector)
    where TIndex : notnull
    where TType : class
{
    private readonly Dictionary<TIndex, TType> _index = data.ToDictionary(keySelector, p => p);

    public bool ContainsKey(TIndex id)
    {
        return _index.ContainsKey(id);
    }
}
