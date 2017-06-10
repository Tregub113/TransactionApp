using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TransactionApp.Domain.Objects
{
    public class TransactionRowModel
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression(@"[0-9]{16}")]
        public string SenderAccountNumber { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression(@"[0-9]{16}")]
        public string RecipientAccountNumber { get; set; }
        [Required]
        [Range(0, Double.MaxValue)]
        public double Summ { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/2017", "1/1/2018")]
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int RowNumber { get; set; }
    }
}
