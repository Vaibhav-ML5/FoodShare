using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodShare.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationClaimFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClaimedAt",
                table: "Donations",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimedBy",
                table: "Donations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Donations",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimedAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "ClaimedBy",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Donations");
        }
    }
}
