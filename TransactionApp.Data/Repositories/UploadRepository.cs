using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransactionApp.Data.Context;
using TransactionApp.Domain.Models;
using System.Linq;

namespace TransactionApp.Data.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        protected readonly DataContext context;

        public UploadRepository(DataContext context)
        {
            if (context == null) throw new InvalidOperationException("DB context not initialized.");
            this.context = context;
        }

        public void CreateTransactionFailure(IEnumerable<ValidationFailureModel> failure)
        {
            context.Failures.AddRange(failure);
            context.SaveChanges();
        }

        public void CreateTransactions(IEnumerable<TransactionModel> entities)
        {
            context.Transactions.AddRange(entities);
            context.SaveChanges(); 
        }

        public void CreateTransactionSummary(TransactionSummaryModel summary)
        {
            context.Summarys.Add(summary);
            context.SaveChanges(); 
        }

        public List<ValidationFailureModel> GetFailuresBySummary(Guid summaryId)
        {
            return context.Failures.Where(f => f.TransactionSummaryId == summaryId)
                                  .Select(f => f).ToList();         
        }

        public List<TransactionModel> GetTransactionsBySummary(Guid summaryId)
        {
            return context.Transactions.Where(t => t.TransactionSummaryId == summaryId)
                .Select(t => t).ToList();
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}
