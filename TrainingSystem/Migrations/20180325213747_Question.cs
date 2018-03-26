using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TrainingSystem.Migrations
{
    public partial class Question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionName",
                table: "Question",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lesson",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Question",
                newName: "QuestionName");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lesson",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
