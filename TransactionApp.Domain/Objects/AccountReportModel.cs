using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TransactionApp.Domain.Models;

namespace TransactionApp.Domain.Objects
{
    public class AccountReportModel
    {
        public string AccountNumber { get; set; }
        public List<TotalByMonth> Totals { get; set; }
    }
    public class TotalByMonth
    {
        public DateTime TransactionDate { get; set; }
        public double Summ { get; set; }
    }
}
