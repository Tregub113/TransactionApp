using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionApp.Core;
using TransactionApp.Domain.Objects;
using Moq;
using System.IO;
using System.Collections;
using System.Linq;
using TransactionApp.Domain.Models;
using TransactionApp.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionApp.Core.Services;

namespace TransactionApp.Test
{
    [TestClass]
    public class ReportServiceTest
    {
        [TestMethod]
        public void GetReport()
        {
            var skip = 0;
            var take = 5;
            var pagingModel = new AccountReportPagingModel();
            var accounts = new List<AccountReportModel>();
            for (var i = skip; i < take; i++)
            {
                var totals = new List<TotalByMonth>();
                for (var j = skip; j<take;j++)
                {
                    totals.Add(new TotalByMonth
                    {
                        Summ = Double.MaxValue / i,
                        TransactionDate = DateTime.Now
                    });
                }
                accounts.Add(new AccountReportModel
                {
                    AccountNumber = "1456325897452144",
                    Totals = totals
                });
            }
            pagingModel.AccountReportList = accounts;
            pagingModel.TotalCount = take;

            var uploadRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            uploadRepositoryMock.Setup(repository => repository.GetAccountReport(skip, take))
                                .Returns(pagingModel);

            reportService = new ReportService(uploadRepositoryMock.Object);
            var result = reportService.GetAccountReport(skip, take);

            Assert.AreEqual(take, result.TotalCount);
            Assert.AreSame(accounts, result.AccountReportList);
        }

        private IReportService reportService;
    }    
}
