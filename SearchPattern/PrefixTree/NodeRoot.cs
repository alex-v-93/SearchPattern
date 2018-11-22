using System.Collections.Generic;
using System.Linq;

namespace SearchPattern.PrefixTree
{
    internal class NodeRoot : Node
    {
        internal NodeRoot() : base(null, '*')
        {
        }
        
        internal void AddWord(string word)
        {
            Node curNode = this;

            curNode = word.Aggregate(curNode, (current, symb) => current.CreateOrGetNode(symb));
            curNode.EndWord();
        }

        
        internal IEnumerable<string> SearchWords(string pattern)
        {
            if (_childs.Count == 0)
                return new List<string>();
            var curNodes = new List<Node> { this };
            var isPrevStar = false;
            foreach (var symb in pattern)
            {
                curNodes = curNodes.Aggregate(new List<Node>(), (list, node) =>
                {
                    list.AddRange(node.SearchNodes(symb, isPrevStar));
                    return list;
                });
                if(curNodes.Count == 0)
                    break;
                isPrevStar = symb == '*' || (isPrevStar && symb == '?');
            }
            var result = curNodes.Aggregate(new List<string>(), (list, node) =>
            {
                list.AddRange(node.GetWords(pattern[pattern.Length - 1] == '*'));
                return list;
            });
            return result;
        }
    }
}
