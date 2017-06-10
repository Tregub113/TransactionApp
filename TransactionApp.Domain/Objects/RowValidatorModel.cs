using System;
using System.Collections.Generic;
using System.Text;
using TransactionApp.Domain.Models;

namespace TransactionApp.Domain.Objects
{
    public class RowValidatorModel
    {
        public bool IsValid { get; set; }
        public ValidationFailureModel FailureModel { get; set; }
    }
}
