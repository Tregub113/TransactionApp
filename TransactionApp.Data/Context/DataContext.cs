using System;
using System.Collections.Generic;
using System.Text;
using TransactionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.Objects;
using System.Linq;

namespace TransactionApp.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Database.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());
        }
        public DbSet<TransactionSummaryModel> Summarys { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<ValidationFailureModel> Failures { get; set; }
        // public DbSet<AccountReportModel> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Ignore<AccountReportModel>();
        }       
    }
}
