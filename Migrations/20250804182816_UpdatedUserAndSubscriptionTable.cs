using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_management_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserAndSubscriptionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PaymentReference",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionType",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            // Add temporary column for PaymentMethod conversion
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodTemp",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            // Convert existing string values to enum integers
            migrationBuilder.Sql(@"
                UPDATE ""Subscriptions"" 
                SET ""PaymentMethodTemp"" = CASE 
                    WHEN ""PaymentMethod"" = 'Cash' THEN 1
                    WHEN ""PaymentMethod"" = 'Credit Card' THEN 2
                    WHEN ""PaymentMethod"" = 'Debit Card' THEN 3
                    WHEN ""PaymentMethod"" = 'Bank Transfer' THEN 4
                    WHEN ""PaymentMethod"" = 'Online' THEN 5
                    ELSE 1
                END");

            // Drop old PaymentMethod column
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Subscriptions");

            // Rename temp column to PaymentMethod
            migrationBuilder.RenameColumn(
                name: "PaymentMethodTemp",
                table: "Subscriptions",
                newName: "PaymentMethod");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Subscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Subscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionType",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            // Add temporary string column for rollback
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodTemp",
                table: "Subscriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "Cash");

            // Convert enum integers back to strings
            migrationBuilder.Sql(@"
                UPDATE ""Subscriptions"" 
                SET ""PaymentMethodTemp"" = CASE 
                    WHEN ""PaymentMethod"" = 1 THEN 'Cash'
                    WHEN ""PaymentMethod"" = 2 THEN 'Credit Card'
                    WHEN ""PaymentMethod"" = 3 THEN 'Debit Card'
                    WHEN ""PaymentMethod"" = 4 THEN 'Bank Transfer'
                    WHEN ""PaymentMethod"" = 5 THEN 'Online'
                    ELSE 'Cash'
                END");

            // Drop enum column
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Subscriptions");

            // Rename temp column back
            migrationBuilder.RenameColumn(
                name: "PaymentMethodTemp",
                table: "Subscriptions",
                newName: "PaymentMethod");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Subscriptions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "Subscriptions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}