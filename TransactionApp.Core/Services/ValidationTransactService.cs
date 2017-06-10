using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TransactionApp.Domain.Models;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Core.Services
{
    public class ValidationTransactService : IValidationTransactService
    {
        public RowValidatorModel RowValidator(TransactionRowModel row, TransactionSummaryModel summaryEntity)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(row);
            var sb = new StringBuilder();
            var validator = new RowValidatorModel
            {
                IsValid = false,
                FailureModel = null
            };

            if (!Validator.TryValidateObject(row, context, results, true))
            {
                foreach (var error in results)
                {
                    sb.AppendFormat("{0} |", error.ErrorMessage);
                }
                validator.FailureModel = new ValidationFailureModel();
                validator.IsValid = false;
                validator.FailureModel.RowNumber = row.RowNumber;
                validator.FailureModel.TransactionSummaryId = summaryEntity.Id;
                validator.FailureModel.Error = sb.ToString();
            }
            else
            {
                validator.IsValid = true;
                validator.FailureModel = null;
            }

            return validator;
        }
    }
}
