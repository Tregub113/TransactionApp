using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TransactionApp.Core
{
    public interface IXlsxStreamProcessor<TRow>
    {
        void Process(Stream stream, Action<TRow> rowAction);        
    }
}
