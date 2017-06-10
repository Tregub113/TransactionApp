using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionApp.Domain.Models;

namespace TransactionApp.Data.Repositories
{
    public interface IUploadRepository
    {        
        void CreateTransactions(IEnumerable<TransactionModel> entities);
        void CreateTransactionSummary(TransactionSummaryModel summary);
        void CreateTransactionFailure(IEnumerable<ValidationFailureModel> failure);
        List<TransactionModel> GetTransactionsBySummary(Guid summaryId);
        List<ValidationFailureModel> GetFailuresBySummary(Guid summaryId);
        void Commit();
    }
}
