using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TransactionApp.Data.Context;

namespace TransactionApp.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TransactionApp.Domain.Models.TransactionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChangedBy");

                    b.Property<DateTime?>("ChangedDate");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("RecipientAccountNumber")
                        .IsRequired();

                    b.Property<string>("SenderAccountNumber")
                        .IsRequired();

                    b.Property<double>("Summ");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<Guid>("TransactionSummaryId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionSummaryId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("TransactionApp.Domain.Models.TransactionSummaryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChangedBy");

                    b.Property<DateTime?>("ChangedDate");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<int>("FailureRow");

                    b.Property<int>("SuccessRow");

                    b.HasKey("Id");

                    b.ToTable("Summarys");
                });

            modelBuilder.Entity("TransactionApp.Domain.Models.ValidationFailureModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChangedBy");

                    b.Property<DateTime?>("ChangedDate");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Error");

                    b.Property<int>("RowNumber");

                    b.Property<Guid>("TransactionSummaryId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionSummaryId");

                    b.ToTable("Failures");
                });

            modelBuilder.Entity("TransactionApp.Domain.Models.TransactionModel", b =>
                {
                    b.HasOne("TransactionApp.Domain.Models.TransactionSummaryModel", "TransactionSummary")
                        .WithMany()
                        .HasForeignKey("TransactionSummaryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TransactionApp.Domain.Models.ValidationFailureModel", b =>
                {
                    b.HasOne("TransactionApp.Domain.Models.TransactionSummaryModel", "TransactionSummary")
                        .WithMany()
                        .HasForeignKey("TransactionSummaryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
