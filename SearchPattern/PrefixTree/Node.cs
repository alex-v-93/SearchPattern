using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchPattern.PrefixTree
{
    internal class Node
    {
        protected ConcurrentDictionary<char, Node> _childs = new ConcurrentDictionary<char, Node>();
        private readonly Node _parent;
        private bool _endWord;
        private readonly char _symbol;

        internal Node(Node parent, char symbol)
        {
            _parent = parent;
            _symbol = symbol;
        }
        #region Добавление слова
        protected internal void EndWord()
        {
            _endWord = true;
        }

        protected internal Node CreateOrGetNode(char symb)
        {
            Node result;
            if (_childs.TryGetValue(symb, out result))
                return result;

            result = _childs.GetOrAdd(symb, new Node(this, symb));
            return result;
        }
        #endregion
        #region Сборка слов
        protected internal List<string> GetWords(bool isLastStar)
        {
            var result = new List<string>();
            var curNodes = new List<Node>() { this };
            var prefix = GetWord(null);

            do
            {
                curNodes = curNodes.Aggregate(new List<Node>(), (list, node) =>
                {
                    if (node._endWord)
                        result.Add(prefix + node.GetWord(this));
                    if (node._childs.Count > 0)
                        list.AddRange(node._childs.Values.ToList());
                    return list;
                });
            } while (curNodes.Count > 0 && isLastStar);
            return result;
        }
        
        private string GetWord(Node stopLevel)
        {
            var result = new StringBuilder();
            var curNode = this;
            while (curNode != stopLevel && (stopLevel != null || curNode._parent != null))
            {
                result.Append(curNode._symbol);
                curNode = curNode._parent;
            }
            return new string(result.ToString().Reverse().ToArray());
        }
        #endregion
        #region Выборка узлов
        protected internal List<Node> SearchNodes(char symb, bool isPrevStar = false)
        {
            var result = new List<Node>();
            if (symb == '*')
                result.Add(this);
            else if (symb == '?')
                result = _childs.Values.ToList();
            else
                result = GetNodes(symb, isPrevStar).ToList();
            return result;
        }

        private IEnumerable<Node> GetNodes(char symb, bool toBottom)
        {
            var result = new List<Node>();
            var curNodes = new List<Node>();
            var accumNodes = new List<Node>();
            var curNode = GetNode(symb, ref curNodes);
            if (curNode != null)
                result.Add(curNode);
            if (!toBottom)
                return result;
            while (curNodes.Count > 0)
            {
                accumNodes.Clear();
                foreach (var node in curNodes)
                {
                    curNode = node.GetNode(symb, ref accumNodes);
                    if (curNode != null)
                        result.Add(curNode);
                }
                curNodes = accumNodes.ToList();
            }
            return result;
        }

        private Node GetNode(char symb, ref List<Node> remain)
        {
            Node result = null;
            foreach (var child in _childs)
            {
                if (child.Key == symb)
                    result = child.Value;
                else
                    remain.Add(child.Value);
            }
            return result;
        }
        #endregion
    }
}