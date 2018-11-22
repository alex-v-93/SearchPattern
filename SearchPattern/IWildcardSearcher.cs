using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchPattern
{
    public interface IWildcardSearcher
    {
        void AddWord(string word);
        IEnumerable<string> SearchWords(string pattern);
    }
}
