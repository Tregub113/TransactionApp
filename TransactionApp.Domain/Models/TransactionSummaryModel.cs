using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionApp.Domain.Models
{
    public class TransactionSummaryModel : BaseModel
    {       
        public int FailureRow { get; set; }
        public int SuccessRow { get; set; }
    }
}
