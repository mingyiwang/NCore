using System;

namespace Core.Document
{

    public class CsvReader : IDisposable
    {

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
