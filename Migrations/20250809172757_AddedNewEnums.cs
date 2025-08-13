using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_management_api.Migrations
{
   /// <inheritdoc />
   public partial class AddedNewEnums : Migration
   {
       /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
       {
           // Add temporary column for Specialization conversion
           migrationBuilder.AddColumn<int>(
               name: "SpecializationTemp",
               table: "Users",
               type: "integer",
               nullable: true,
               defaultValue: null);

           // Convert existing string values to enum integers
           migrationBuilder.Sql(@"
               UPDATE ""Users"" 
               SET ""SpecializationTemp"" = CASE 
                   WHEN ""Specialization"" = 'Bodybuilding' THEN 1
                   WHEN ""Specialization"" = 'Nutritionist' THEN 2
                   WHEN ""Specialization"" = 'Cardio' THEN 3
                   WHEN ""Specialization"" = 'Yoga' THEN 4
                   ELSE NULL
               END");

           // Drop old Specialization column
           migrationBuilder.DropColumn(
               name: "Specialization",
               table: "Users");

           // Rename temp column to Specialization
           migrationBuilder.RenameColumn(
               name: "SpecializationTemp",
               table: "Users",
               newName: "Specialization");
       }

       /// <inheritdoc />
       protected override void Down(MigrationBuilder migrationBuilder)
       {
           // Add temporary string column for rollback
           migrationBuilder.AddColumn<string>(
               name: "SpecializationTemp",
               table: "Users",
               type: "character varying(100)",
               maxLength: 100,
               nullable: true,
               defaultValue: null);

           // Convert enum integers back to strings
           migrationBuilder.Sql(@"
               UPDATE ""Users"" 
               SET ""SpecializationTemp"" = CASE 
                   WHEN ""Specialization"" = 1 THEN 'Bodybuilding'
                   WHEN ""Specialization"" = 2 THEN 'Nutritionist'
                   WHEN ""Specialization"" = 3 THEN 'Cardio'
                   WHEN ""Specialization"" = 4 THEN 'Yoga'
                   ELSE NULL
               END");

           // Drop enum column
           migrationBuilder.DropColumn(
               name: "Specialization",
               table: "Users");

           // Rename temp column back to Specialization
           migrationBuilder.RenameColumn(
               name: "SpecializationTemp",
               table: "Users",
               newName: "Specialization");
       }
   }
}