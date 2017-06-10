using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TransactionApp.Data.Repositories;
using TransactionApp.Domain.Models;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Core.Services
{
    public class UploadService : IUploadService
    {
        private const int WriteChunkSize = 1000;
        private readonly IUploadRepository uploadRepository;
        private readonly IXlsxStreamProcessor<TransactionRowModel> xlsxStreamProcessor;
        private readonly IValidationTransactService validationService;
        public UploadService(IUploadRepository uploadRepository,
                IXlsxStreamProcessor<TransactionRowModel> xlsxStreamProcessor,
                IValidationTransactService validationService
            )
        {
            this.uploadRepository = uploadRepository;
            this.xlsxStreamProcessor = xlsxStreamProcessor;
            this.validationService = validationService;
        }

        public TransactionSummaryModel Upload(Stream xlStream)
        {
            var transactionEntities = new List<TransactionModel>();
            var failureEntities = new List<ValidationFailureModel>();
            var summaryEntity = CreateSumary();


            xlsxStreamProcessor.Process(xlStream, row =>
            {               
                var validator = validationService.RowValidator(row, summaryEntity);
                if (!validator.IsValid)
                {
                    var entity = validator.FailureModel;
                    if (entity != null)
                        failureEntities.Add(entity);

                    if (failureEntities.Count >= WriteChunkSize)
                        HandleWriteAndFlushFailure(failureEntities);
                }
                else
                {
                    var entity = GetTransaction(row, summaryEntity);
                    if (entity != null)
                        transactionEntities.Add(entity);

                    if (transactionEntities.Count >= WriteChunkSize)
                        HandleWriteAndFlushTransact(transactionEntities);
                }
            });
            HandleWriteAndFlushTransact(transactionEntities);
            HandleWriteAndFlushFailure(failureEntities);
            return SummaryFilling(summaryEntity);
        }

        private TransactionSummaryModel SummaryFilling(TransactionSummaryModel summaryEntity)
        {
            var failureList = uploadRepository.GetFailuresBySummary(summaryEntity.Id);
            var transactList = uploadRepository.GetTransactionsBySummary(summaryEntity.Id);
            summaryEntity.FailureRow = failureList.Count;
            summaryEntity.SuccessRow = transactList.Count;
            uploadRepository.Commit();
            return summaryEntity;
        }

        private TransactionSummaryModel CreateSumary()
        {
            var summaryEntity = new TransactionSummaryModel { Id = new Guid() };
            uploadRepository.CreateTransactionSummary(summaryEntity);
            return summaryEntity;
        }

        private void HandleWriteAndFlushTransact(List<TransactionModel> entities)
        {
            uploadRepository.CreateTransactions(entities);
            entities.Clear();            
        }
        private void HandleWriteAndFlushFailure(List<ValidationFailureModel> entities)
        {
            uploadRepository.CreateTransactionFailure(entities);
            entities.Clear();
        }
        private TransactionModel GetTransaction(TransactionRowModel row, TransactionSummaryModel summaryEntity)
        {
            var entity = new TransactionModel();           
            entity.SenderAccountNumber = row.SenderAccountNumber;
            entity.RecipientAccountNumber = row.RecipientAccountNumber;
            entity.Summ = row.Summ;
            entity.TransactionDate = row.TransactionDate;
            entity.Description = row.Description;
            entity.TransactionSummaryId = summaryEntity.Id;
            return entity;
        }
    }
}
