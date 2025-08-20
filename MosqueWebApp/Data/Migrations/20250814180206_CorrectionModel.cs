using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MosqueWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminMosqueeId",
                table: "Mosquees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImamId",
                table: "Mosquees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MosqueeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mosquees_AdminMosqueeId",
                table: "Mosquees",
                column: "AdminMosqueeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mosquees_ImamId",
                table: "Mosquees",
                column: "ImamId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MosqueeId",
                table: "AspNetUsers",
                column: "MosqueeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Mosquees_MosqueeId",
                table: "AspNetUsers",
                column: "MosqueeId",
                principalTable: "Mosquees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mosquees_AspNetUsers_AdminMosqueeId",
                table: "Mosquees",
                column: "AdminMosqueeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mosquees_AspNetUsers_ImamId",
                table: "Mosquees",
                column: "ImamId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Mosquees_MosqueeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Mosquees_AspNetUsers_AdminMosqueeId",
                table: "Mosquees");

            migrationBuilder.DropForeignKey(
                name: "FK_Mosquees_AspNetUsers_ImamId",
                table: "Mosquees");

            migrationBuilder.DropIndex(
                name: "IX_Mosquees_AdminMosqueeId",
                table: "Mosquees");

            migrationBuilder.DropIndex(
                name: "IX_Mosquees_ImamId",
                table: "Mosquees");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MosqueeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdminMosqueeId",
                table: "Mosquees");

            migrationBuilder.DropColumn(
                name: "ImamId",
                table: "Mosquees");

            migrationBuilder.DropColumn(
                name: "MosqueeId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
