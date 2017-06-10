using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionApp.Domain.Models
{
    public class TransactionModel : BaseModel
    {
        [Required]
        public string SenderAccountNumber { get; set; }

        [Required]
        public string RecipientAccountNumber { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double Summ { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }
        public String Description { get; set; }

        [ForeignKey("TransactionSummary")]
        public virtual Guid TransactionSummaryId { get; set; }
        public virtual TransactionSummaryModel TransactionSummary { get; set; }
    }
}