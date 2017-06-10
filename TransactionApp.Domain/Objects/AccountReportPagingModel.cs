using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionApp.Domain.Objects
{
    public class AccountReportPagingModel
    {
        public List<AccountReportModel> AccountReportList { get; set; }
        public int TotalCount { get; set; }
    }
}
