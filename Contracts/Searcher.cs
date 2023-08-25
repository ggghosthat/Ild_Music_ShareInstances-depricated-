namespace ShareInstances;

public interface ISearchResult
{}

public enum SearchContent
{
    ARTIST,
    PLAYLIST,
    TRACK
}

public interface ISearcher<out T>
{
    public ISearchResult Search(string query);

    public ISearchResult Search(string query, SearchContent content);
}
