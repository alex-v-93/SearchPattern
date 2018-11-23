using System.Collections.Generic;

namespace SearchPattern.PrefixTree
{
    internal class WildcardSearcher : IWildcardSearcher
    {
        private readonly NodeRoot _prefixTree;
        internal WildcardSearcher()
        {
            _prefixTree = new NodeRoot();
        }
        public void AddWord(string word)
        {
            _prefixTree.AddWord(word);
        }

        public IEnumerable<string> SearchWords(string pattern)
        {
            return _prefixTree.SearchWords(pattern);
        }
    }
}
