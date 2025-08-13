using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_management_api.Migrations
{
   /// <inheritdoc />
   public partial class AddedRequirementToMoreFields : Migration
   {
       /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
       {
           // Add temporary column for Gender conversion
           migrationBuilder.AddColumn<int>(
               name: "GenderTemp",
               table: "Users",
               type: "integer",
               nullable: false,
               defaultValue: 1); // Default to Male

           // Convert existing string values to enum integers
           migrationBuilder.Sql(@"
               UPDATE ""Users"" 
               SET ""GenderTemp"" = CASE 
                   WHEN ""Gender"" = 'Male' THEN 1
                   WHEN ""Gender"" = 'Female' THEN 2
                   ELSE 1
               END");

           // Drop old Gender column
           migrationBuilder.DropColumn(
               name: "Gender",
               table: "Users");

           // Rename temp column to Gender
           migrationBuilder.RenameColumn(
               name: "GenderTemp",
               table: "Users",
               newName: "Gender");

           // Handle TrainerMembers changes
           migrationBuilder.AlterColumn<string>(
               name: "TrainingGoals",
               table: "TrainerMembers",
               type: "character varying(500)",
               maxLength: 500,
               nullable: true,
               oldClrType: typeof(string),
               oldType: "character varying(500)",
               oldMaxLength: 500);

           migrationBuilder.AlterColumn<string>(
               name: "Notes",
               table: "TrainerMembers",
               type: "character varying(500)",
               maxLength: 500,
               nullable: true,
               oldClrType: typeof(string),
               oldType: "character varying(500)",
               oldMaxLength: 500);
       }

       /// <inheritdoc />
       protected override void Down(MigrationBuilder migrationBuilder)
       {
           // Reverse the Gender conversion
           migrationBuilder.AddColumn<string>(
               name: "GenderTemp",
               table: "Users",
               type: "character varying(10)",
               maxLength: 10,
               nullable: false,
               defaultValue: "Male");

           migrationBuilder.Sql(@"
               UPDATE ""Users"" 
               SET ""GenderTemp"" = CASE 
                   WHEN ""Gender"" = 1 THEN 'Male'
                   WHEN ""Gender"" = 2 THEN 'Female'
                   ELSE 'Male'
               END");

           migrationBuilder.DropColumn(
               name: "Gender",
               table: "Users");

           migrationBuilder.RenameColumn(
               name: "GenderTemp",
               table: "Users",
               newName: "Gender");

           // Reverse TrainerMembers changes
           migrationBuilder.AlterColumn<string>(
               name: "TrainingGoals",
               table: "TrainerMembers",
               type: "character varying(500)",
               maxLength: 500,
               nullable: false,
               defaultValue: "",
               oldClrType: typeof(string),
               oldType: "character varying(500)",
               oldMaxLength: 500,
               oldNullable: true);

           migrationBuilder.AlterColumn<string>(
               name: "Notes",
               table: "TrainerMembers",
               type: "character varying(500)",
               maxLength: 500,
               nullable: false,
               defaultValue: "",
               oldClrType: typeof(string),
               oldType: "character varying(500)",
               oldMaxLength: 500,
               oldNullable: true);
       }
   }
}