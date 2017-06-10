using System;
using System.Collections.Generic;
using System.Text;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Data.Repositories
{
    public interface IReportRepository
    {
        AccountReportPagingModel GetAccountReport(int skip, int take);
    }
}
