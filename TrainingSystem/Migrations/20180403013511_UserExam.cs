using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TrainingSystem.Migrations
{
    public partial class UserExam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserExam",
                columns: table => new
                {
                    UserExamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    SubmissionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExam", x => x.UserExamId);
                    table.ForeignKey(
                        name: "FK_UserExam_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExam_Exam_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exam",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExamChoice",
                columns: table => new
                {
                    UserExamChoiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionChoiceId = table.Column<int>(nullable: false),
                    UserExamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExamChoice", x => x.UserExamChoiceId);
                    table.ForeignKey(
                        name: "FK_UserExamChoice_QuestionChoice_QuestionChoiceId",
                        column: x => x.QuestionChoiceId,
                        principalTable: "QuestionChoice",
                        principalColumn: "QuestionChoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExamChoice_UserExam_UserExamId",
                        column: x => x.UserExamId,
                        principalTable: "UserExam",
                        principalColumn: "UserExamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_EmployeeId",
                table: "UserExam",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_ExamId",
                table: "UserExam",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamChoice_QuestionChoiceId",
                table: "UserExamChoice",
                column: "QuestionChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamChoice_UserExamId",
                table: "UserExamChoice",
                column: "UserExamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserExamChoice");

            migrationBuilder.DropTable(
                name: "UserExam");
        }
    }
}
