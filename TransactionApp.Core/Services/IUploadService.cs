using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TransactionApp.Domain.Models;

namespace TransactionApp.Core.Services
{
    public interface IUploadService
    {
        TransactionSummaryModel Upload(Stream xlStream);
    }
}
