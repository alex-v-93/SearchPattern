using SearchPattern.IO;
using SearchPattern.PrefixTree;

namespace SearchPattern
{
    public static class SearcherFactory
    {
        private static IWildcardSearcher _searcher;

        public static IWildcardSearcher GetSearcher()
        {
            return _searcher ?? (_searcher = new WildcardSearcher());
        }

        public static IDictionaryReader GetDictionaryReader(string path)
        {
            return new DictionaryReader(path);
        }
    }
}
