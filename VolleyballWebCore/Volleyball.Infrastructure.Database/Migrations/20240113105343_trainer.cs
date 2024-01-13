using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volleyball.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class trainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTemplates_Users_TrainerId",
                table: "MessageTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingGroups_Users_TrainerId",
                table: "TrainingGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingParticipants_Users_TrainerId",
                table: "TrainingParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Users_TrainerId",
                table: "Trainings");

            migrationBuilder.CreateTable(
                name: "Trainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTemplates_Trainer_TrainerId",
                table: "MessageTemplates",
                column: "TrainerId",
                principalTable: "Trainer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingGroups_Trainer_TrainerId",
                table: "TrainingGroups",
                column: "TrainerId",
                principalTable: "Trainer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingParticipants_Trainer_TrainerId",
                table: "TrainingParticipants",
                column: "TrainerId",
                principalTable: "Trainer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Trainer_TrainerId",
                table: "Trainings",
                column: "TrainerId",
                principalTable: "Trainer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTemplates_Trainer_TrainerId",
                table: "MessageTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingGroups_Trainer_TrainerId",
                table: "TrainingGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingParticipants_Trainer_TrainerId",
                table: "TrainingParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Trainer_TrainerId",
                table: "Trainings");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTemplates_Users_TrainerId",
                table: "MessageTemplates",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingGroups_Users_TrainerId",
                table: "TrainingGroups",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingParticipants_Users_TrainerId",
                table: "TrainingParticipants",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Users_TrainerId",
                table: "Trainings",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
