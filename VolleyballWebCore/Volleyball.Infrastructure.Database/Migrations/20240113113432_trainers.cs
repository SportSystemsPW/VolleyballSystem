using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volleyball.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class trainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainer",
                table: "Trainer");

            migrationBuilder.RenameTable(
                name: "Trainer",
                newName: "Trainers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainers",
                table: "Trainers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTemplates_Trainers_TrainerId",
                table: "MessageTemplates",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingGroups_Trainers_TrainerId",
                table: "TrainingGroups",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingParticipants_Trainers_TrainerId",
                table: "TrainingParticipants",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Trainers_TrainerId",
                table: "Trainings",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTemplates_Trainers_TrainerId",
                table: "MessageTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingGroups_Trainers_TrainerId",
                table: "TrainingGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingParticipants_Trainers_TrainerId",
                table: "TrainingParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Trainers_TrainerId",
                table: "Trainings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainers",
                table: "Trainers");

            migrationBuilder.RenameTable(
                name: "Trainers",
                newName: "Trainer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainer",
                table: "Trainer",
                column: "Id");

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
    }
}
