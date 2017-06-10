using System;
using System.Collections.Generic;
using System.Text;
using TransactionApp.Domain.Models;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Core.Services
{
    public interface IValidationTransactService
    {
        RowValidatorModel RowValidator(TransactionRowModel row, TransactionSummaryModel summaryEntity);
    }
}
