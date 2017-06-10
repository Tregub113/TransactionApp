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
    public class ValidationTransactionServiceTest
    {
        [TestMethod]
        public void ValidationRowTest()
        {
            validationService = new ValidationTransactService();
            TransactionRowModel row = new TransactionRowModel
            {
                SenderAccountNumber = "2554255635588473",
                RecipientAccountNumber = "2568785964823541",
                Summ = Double.MaxValue,
                TransactionDate = DateTime.Now             
            };
            TransactionSummaryModel summaryModel = new TransactionSummaryModel();
            
            var result = validationService.RowValidator(row, summaryModel).IsValid;

            Assert.IsTrue(result);            
        }
        private IValidationTransactService validationService;
    }
}
