using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volleyball.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class trainingOrganizer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageTemplates_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingGroups_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingParticipants_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainings_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingGroupTrainingParticipants",
                columns: table => new
                {
                    TrainingGroupId = table.Column<int>(type: "int", nullable: false),
                    TrainingParticipantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGroupTrainingParticipants", x => new { x.TrainingGroupId, x.TrainingParticipantId });
                    table.ForeignKey(
                        name: "FK_TrainingGroupTrainingParticipants_TrainingGroups_TrainingGroupId",
                        column: x => x.TrainingGroupId,
                        principalTable: "TrainingGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingGroupTrainingParticipants_TrainingParticipants_TrainingParticipantId",
                        column: x => x.TrainingParticipantId,
                        principalTable: "TrainingParticipants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingTrainingParticipants",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainingParticipantId = table.Column<int>(type: "int", nullable: false),
                    Presence = table.Column<bool>(type: "bit", nullable: false),
                    MessageSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTrainingParticipants", x => new { x.TrainingId, x.TrainingParticipantId });
                    table.ForeignKey(
                        name: "FK_TrainingTrainingParticipants_TrainingParticipants_TrainingParticipantId",
                        column: x => x.TrainingParticipantId,
                        principalTable: "TrainingParticipants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingTrainingParticipants_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageTemplates_TrainerId",
                table: "MessageTemplates",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGroups_TrainerId",
                table: "TrainingGroups",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGroupTrainingParticipants_TrainingParticipantId",
                table: "TrainingGroupTrainingParticipants",
                column: "TrainingParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingParticipants_TrainerId",
                table: "TrainingParticipants",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TrainerId",
                table: "Trainings",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTrainingParticipants_TrainingParticipantId",
                table: "TrainingTrainingParticipants",
                column: "TrainingParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageTemplates");

            migrationBuilder.DropTable(
                name: "TrainingGroupTrainingParticipants");

            migrationBuilder.DropTable(
                name: "TrainingTrainingParticipants");

            migrationBuilder.DropTable(
                name: "TrainingGroups");

            migrationBuilder.DropTable(
                name: "TrainingParticipants");

            migrationBuilder.DropTable(
                name: "Trainings");
        }
    }
}
