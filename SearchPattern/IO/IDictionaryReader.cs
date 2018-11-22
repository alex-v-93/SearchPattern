using System;

namespace SearchPattern
{
    public interface IDictionaryReader : IDisposable
    {
        string GetWord();
    }
}
