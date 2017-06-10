using System;
using System.Collections.Generic;
using System.Text;
using TransactionApp.Data.Repositories;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }

        public AccountReportPagingModel GetAccountReport(int skip, int take)
        {
            return reportRepository.GetAccountReport(skip, take);
        }
    }
}
