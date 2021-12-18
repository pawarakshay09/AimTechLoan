using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace aimtechloansystem.Data.Migrations
{
    public partial class EMI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Fee = table.Column<long>(nullable: false),
                    LastDate = table.Column<DateTime>(nullable: false),
                    LoanAmount = table.Column<long>(nullable: false),
                    LoanBalance = table.Column<float>(nullable: false),
                    Mobile = table.Column<long>(nullable: false),
                    Months = table.Column<int>(nullable: false),
                    NextPaid = table.Column<DateTime>(nullable: false),
                    Percent = table.Column<float>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EMI",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(nullable: false),
                    EMIAmount = table.Column<float>(nullable: false),
                    EMIDate = table.Column<DateTime>(nullable: false),
                    LateFee = table.Column<float>(nullable: false),
                    LoanBal = table.Column<float>(nullable: false),
                    Mudal = table.Column<float>(nullable: false),
                    Vyaj = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMI", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EMI_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EMI_CustomerID",
                table: "EMI",
                column: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMI");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
