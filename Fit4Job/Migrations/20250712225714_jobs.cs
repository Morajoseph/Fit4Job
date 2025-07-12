using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fit4Job.Migrations
{
    /// <inheritdoc />
    public partial class jobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "company_tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobApplicationId",
                table: "company_task_submissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "company_exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobApplicationId",
                table: "company_exam_attempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    JobType = table.Column<string>(type: "varchar(20)", nullable: false),
                    WorkLocationType = table.Column<string>(type: "varchar(20)", nullable: false),
                    EducationLevel = table.Column<string>(type: "varchar(20)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SalaryRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearsOfExperience = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jobs_company_profiles_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ExamAttemptId = table.Column<int>(type: "int", nullable: true),
                    TaskSubmissionId = table.Column<int>(type: "int", nullable: true),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_JobApplications_company_exam_attempts_ExamAttemptId",
                        column: x => x.ExamAttemptId,
                        principalTable: "company_exam_attempts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobApplications_company_task_submissions_TaskSubmissionId",
                        column: x => x.TaskSubmissionId,
                        principalTable: "company_task_submissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobApplications_jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_company_tasks_JobId",
                table: "company_tasks",
                column: "JobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_exams_JobId",
                table: "company_exams",
                column: "JobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ExamAttemptId",
                table: "JobApplications",
                column: "ExamAttemptId",
                unique: true,
                filter: "[ExamAttemptId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobId",
                table: "JobApplications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_TaskSubmissionId",
                table: "JobApplications",
                column: "TaskSubmissionId",
                unique: true,
                filter: "[TaskSubmissionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId_JobId",
                table: "JobApplications",
                columns: new[] { "UserId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyId",
                table: "jobs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsActive_DeletedAt",
                table: "jobs",
                columns: new[] { "IsActive", "DeletedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_company_exams_jobs_JobId",
                table: "company_exams",
                column: "JobId",
                principalTable: "jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_company_tasks_jobs_JobId",
                table: "company_tasks",
                column: "JobId",
                principalTable: "jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_company_exams_jobs_JobId",
                table: "company_exams");

            migrationBuilder.DropForeignKey(
                name: "FK_company_tasks_jobs_JobId",
                table: "company_tasks");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropIndex(
                name: "IX_company_tasks_JobId",
                table: "company_tasks");

            migrationBuilder.DropIndex(
                name: "IX_company_exams_JobId",
                table: "company_exams");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "company_tasks");

            migrationBuilder.DropColumn(
                name: "JobApplicationId",
                table: "company_task_submissions");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "company_exams");

            migrationBuilder.DropColumn(
                name: "JobApplicationId",
                table: "company_exam_attempts");
        }
    }
}
