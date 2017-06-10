using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransactionApp.Domain.Models
{
    public class ValidationFailureModel : BaseModel
    {
        public int RowNumber { get; set; }
        public string Error { get; set; }

        [ForeignKey("TransactionSummary")]
        public Guid TransactionSummaryId { get; set; }
        public virtual TransactionSummaryModel TransactionSummary { get; set; }
    }
}
