using System;

namespace Core.Csv
{

    public class CsvReader : IDisposable
    {

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
