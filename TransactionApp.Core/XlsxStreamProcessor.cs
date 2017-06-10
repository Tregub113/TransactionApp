using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;
using TransactionApp.Domain.Objects;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices;

namespace TransactionApp.Core
{
    public class XlsxTransactionRawProcessor<TRow> : IXlsxStreamProcessor<TRow>
        where TRow : TransactionRowModel
    {
        public void Process(Stream stream, Action<TRow> rowAction)
        {
            try
            {
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                    for (var rowNumber = workSheet.Dimension.Start.Row; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                    {

                        var row = new TransactionRowModel()
                        {
                            SenderAccountNumber = workSheet.GetValue<string>(rowNumber, 1),
                            RecipientAccountNumber = workSheet.GetValue<string>(rowNumber, 2),
                            Summ = Double.TryParse(workSheet.GetValue<string>(rowNumber, 3), out double result) ? result : -1,
                            Description = workSheet.GetValue<string>(rowNumber, 5),
                            RowNumber = rowNumber
                        };
                        try
                        {
                            row.TransactionDate = workSheet.GetValue<DateTime>(rowNumber, 4);
                        }
                        catch
                        {
                            row.TransactionDate = DateTime.MinValue;
                        }

                        rowAction((TRow)row);
                    }
                }
            }
            catch(COMException ex)
            {
                return;
            }
        }
    }
}

