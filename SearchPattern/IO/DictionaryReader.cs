using System.IO;
using System.Text;

namespace SearchPattern.IO
{
    internal class DictionaryReader : StreamReader, IDictionaryReader
    {
        internal DictionaryReader(string path) : base(path)
        {
            FillBuffer();
        }

        readonly char[] _buffer = new char[4096];
        private int _countRead;
        private int _currentPosition;
        string IDictionaryReader.GetWord()
        {
            var resultCharArray = new StringBuilder();
            var isStop = false;
            while(!isStop)
            {
                var s = ReadSymbol();
                if (s == '\0')
                    break;
                if(char.IsLetter(s))
                {
                    resultCharArray.Append(s);
                }
                else
                {
                    if (resultCharArray.Length > 0)
                        isStop = true;
                }
            }
            
            return resultCharArray.ToString();
        }

        private char ReadSymbol()
        {
            if (_currentPosition == _countRead)
                FillBuffer();
            return _countRead == 0 ? '\0' : _buffer[_currentPosition++];
        }

        private void FillBuffer()
        {
            _currentPosition = 0;
            _countRead = ReadBlock(_buffer, 0, _buffer.Length);
        }

    }
}
