using System;
using System.Collections.Generic;
using System.Text;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Core.Services
{
    public interface IReportService
    {
        AccountReportPagingModel GetAccountReport(int skip, int take);
    }
}
