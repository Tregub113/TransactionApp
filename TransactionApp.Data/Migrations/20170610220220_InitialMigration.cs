using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionApp.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summarys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChangedBy = table.Column<Guid>(nullable: false),
                    ChangedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    FailureRow = table.Column<int>(nullable: false),
                    SuccessRow = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summarys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChangedBy = table.Column<Guid>(nullable: false),
                    ChangedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RecipientAccountNumber = table.Column<string>(nullable: false),
                    SenderAccountNumber = table.Column<string>(nullable: false),
                    Summ = table.Column<double>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionSummaryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Summarys_TransactionSummaryId",
                        column: x => x.TransactionSummaryId,
                        principalTable: "Summarys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Failures",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChangedBy = table.Column<Guid>(nullable: false),
                    ChangedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Error = table.Column<string>(nullable: true),
                    RowNumber = table.Column<int>(nullable: false),
                    TransactionSummaryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Failures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Failures_Summarys_TransactionSummaryId",
                        column: x => x.TransactionSummaryId,
                        principalTable: "Summarys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionSummaryId",
                table: "Transactions",
                column: "TransactionSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Failures_TransactionSummaryId",
                table: "Failures",
                column: "TransactionSummaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Failures");

            migrationBuilder.DropTable(
                name: "Summarys");
        }
    }
}
