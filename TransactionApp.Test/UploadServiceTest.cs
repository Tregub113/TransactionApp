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
    public class UploadServiceTest
    {
        [TestMethod]
        public void UploadChunkTest()
        {
            var savedCount = 0;
            var iterationsNumber = 150;
            var uploadRepositoryMock = new Mock<IUploadRepository>(MockBehavior.Strict);
            uploadRepositoryMock.Setup(repository => repository.CreateTransactions(It.IsAny<IEnumerable<TransactionModel>>()))
                                .Callback<IEnumerable<TransactionModel>>(r =>
                                {
                                    savedCount += r.Count();
                                });
            uploadRepositoryMock.Setup(repository => repository.CreateTransactionSummary(It.IsAny<TransactionSummaryModel>()));
            uploadRepositoryMock.Setup(repository => repository.CreateTransactionFailure(It.IsAny<IEnumerable<ValidationFailureModel>>()));
            uploadRepositoryMock.Setup(repository => repository.GetFailuresBySummary(It.IsAny<Guid>()))
                .Returns(new List<ValidationFailureModel> { new ValidationFailureModel() });
            uploadRepositoryMock.Setup(repository => repository.GetTransactionsBySummary(It.IsAny<Guid>()))
                .Returns(new List<TransactionModel> { new TransactionModel() });


            uploadRepositoryMock.Setup(repository => repository.Commit());

            var xlsxProcessorMock = new Mock<IXlsxStreamProcessor<TransactionRowModel>>(MockBehavior.Strict);
            xlsxProcessorMock.Setup(service => service.Process(It.IsAny<Stream>(), It.IsAny<Action<TransactionRowModel>>()))
                .Callback<Stream, Action<TransactionRowModel>>((s, processed) =>
                {
                    foreach (var i in Enumerable.Range(0, iterationsNumber))
                    {
                        processed(new TransactionRowModel
                        {
                            SenderAccountNumber = "1233212312134534",
                            RecipientAccountNumber = "1234567898524658",
                            Summ = 25,
                            TransactionDate = DateTime.Now,
                            Description = "QQ",                            
                            RowNumber = i
                        });
                    }
                });
            var validationTransactService = new Mock<IValidationTransactService>(MockBehavior.Strict);
            validationTransactService.Setup(repository => repository.RowValidator(It.IsAny<TransactionRowModel>(), It.IsAny<TransactionSummaryModel>()))
            .Returns(new RowValidatorModel
            {
                IsValid = true,
                FailureModel = null
            }
            );

            var uploadService = new UploadService(uploadRepositoryMock.Object, xlsxProcessorMock.Object, validationTransactService.Object);
            var result = uploadService.Upload(new MemoryStream()).SuccessRow;
            Assert.AreEqual(iterationsNumber, savedCount);
        }
    }
}
