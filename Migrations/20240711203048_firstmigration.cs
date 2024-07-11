using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DemoTask.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clientMaster",
                columns: table => new
                {
                    nId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sIcNumber = table.Column<string>(type: "longtext", nullable: false),
                    sCustomerName = table.Column<string>(type: "longtext", nullable: false),
                    sMobileNo = table.Column<string>(type: "longtext", nullable: false),
                    sEmail = table.Column<string>(type: "longtext", nullable: false),
                    bPrivacyPolicy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    sPin = table.Column<string>(type: "longtext", nullable: false),
                    sBiometric = table.Column<string>(type: "longtext", nullable: true),
                    dtCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dtLastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientMaster", x => x.nId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "otpMasters",
                columns: table => new
                {
                    nId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sIcNumber = table.Column<string>(type: "longtext", nullable: false),
                    sOtpFor = table.Column<string>(type: "longtext", nullable: false),
                    nOtpType = table.Column<short>(type: "smallint", nullable: false),
                    sOtp = table.Column<string>(type: "longtext", nullable: false),
                    bOtpVerify = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    nResendOTPCount = table.Column<short>(type: "smallint", nullable: false),
                    dtOtpGentnTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otpMasters", x => x.nId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientMaster");

            migrationBuilder.DropTable(
                name: "otpMasters");
        }
    }
}
