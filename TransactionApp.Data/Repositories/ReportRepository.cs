using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TransactionApp.Data.Context;
using TransactionApp.Domain.Objects;

namespace TransactionApp.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        protected readonly string connectionString;

        public ReportRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        const string sqlCount = @"Select count(*)
                                    From(
	                                    Select *
	                                    From(
	                                    Select RecipientAccountNumber
			                                    from dbo.Transactions
			                                    Group by RecipientAccountNumber
	                                    union all Select SenderAccountNumber
				                                    From dbo.Transactions
				                                    Group by SenderAccountNumber) as uni
	                                    Group By uni.RecipientAccountNumber
                                    ) as summ";
        const string sql = @"Select AccountNumber, AccountNumber as splitId, TransactionDate, Summ
                                From(
	                                Select  row_number() over (order by uni.RecipientAccountNumber) as rownum, uni.RecipientAccountNumber As AccountNumber, uni.TransactionDate, sum(uni.Summ) as Summ
	                                From(
	                                Select RecipientAccountNumber, dateadd(M, datediff(M, 0, TransactionDate), 0) as TransactionDate, SUM(Summ) as Summ
			                                from dbo.Transactions
			                                Group by TransactionDate, RecipientAccountNumber
	                                union all Select SenderAccountNumber, dateadd(M, datediff(M, 0, TransactionDate), 0) as TransactionDate, -SUM(Summ) as Summ
				                                From dbo.Transactions
				                                Group by TransactionDate, SenderAccountNumber) as uni
	                                Group By uni.TransactionDate, uni.RecipientAccountNumber
	                                )as AccountReportView
                                Where AccountReportView.rownum > @skip and AccountReportView.rownum <= @take 
                                ";
        public AccountReportPagingModel GetAccountReport(int skip, int take)
        {
            var pagingModel = new AccountReportPagingModel();
            var accounts = new List<AccountReportModel>();
            int accountsCount;
            using (IDbConnection db = new SqlConnection(connectionString))
            {                
                var lookup = new Dictionary<string, AccountReportModel>();

                accountsCount = db.Query<int>(sqlCount).FirstOrDefault();
                db.Query<AccountReportModel, TotalByMonth, AccountReportModel>(sql, (r, t) => {
                    AccountReportModel mem;
                    if (!lookup.TryGetValue(r.AccountNumber, out mem))
                        lookup.Add(r.AccountNumber, mem = r);
                    if (mem.Totals == null)
                        mem.Totals = new List<TotalByMonth>();
                    mem.Totals.Add(t);                    
                    return mem;
                }, 
                new {@skip = skip, @take = take+skip+1},
                splitOn: "splitId");

                accounts = lookup.Values.ToList();                
            }
            pagingModel.AccountReportList = accounts;
            pagingModel.TotalCount = accountsCount;

            return pagingModel;
        }
    }
}