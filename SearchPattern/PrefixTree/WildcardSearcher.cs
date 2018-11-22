﻿using System.Collections.Generic;

namespace SearchPattern.PrefixTree
{
    internal class WildcardSearcher : IWildcardSearcher
    {
        private readonly NodeRoot _prefixThree;
        internal WildcardSearcher()
        {
            _prefixThree = new NodeRoot();
        }
        public void AddWord(string word)
        {
            _prefixThree.AddWord(word);
        }

        public IEnumerable<string> SearchWords(string pattern)
        {
            return _prefixThree.SearchWords(pattern);
        }
    }
}
