using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning_Outcomes.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstructorInstances",
                columns: table => new
                {
                    InstructorInstancesID = table.Column<int>(nullable: false),
                    InstructorUserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorInstances", x => x.InstructorInstancesID);
                });

            migrationBuilder.CreateTable(
                name: "CourseInstances",
                columns: table => new
                {
                    CourseInstancesID = table.Column<int>(nullable: false),
                    CourseName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Dept = table.Column<string>(nullable: false),
                    Semester = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    InstructorInstancesID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseInstances", x => x.CourseInstancesID);
                    table.ForeignKey(
                        name: "FK_CourseInstances_InstructorInstances_InstructorInstancesID",
                        column: x => x.InstructorInstancesID,
                        principalTable: "InstructorInstances",
                        principalColumn: "InstructorInstancesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomeInstances",
                columns: table => new
                {
                    LearningOutcomeInstancesID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CourseInstancesID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomeInstances", x => x.LearningOutcomeInstancesID);
                    table.ForeignKey(
                        name: "FK_LearningOutcomeInstances_CourseInstances_CourseInstancesID",
                        column: x => x.CourseInstancesID,
                        principalTable: "CourseInstances",
                        principalColumn: "CourseInstancesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstances_InstructorInstancesID",
                table: "CourseInstances",
                column: "InstructorInstancesID");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomeInstances_CourseInstancesID",
                table: "LearningOutcomeInstances",
                column: "CourseInstancesID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningOutcomeInstances");

            migrationBuilder.DropTable(
                name: "CourseInstances");

            migrationBuilder.DropTable(
                name: "InstructorInstances");
        }
    }
}
