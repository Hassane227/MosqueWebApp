using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MosqueWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContenusReligieux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContenuTexte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlMedia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatePublication = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Langue = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContenusReligieux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContenusReligieux_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mosquees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mosquees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BesoinsMateriels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MosqueeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BesoinsMateriels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BesoinsMateriels_Mosquees_MosqueeId",
                        column: x => x.MosqueeId,
                        principalTable: "Mosquees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MosqueeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Anonyme = table.Column<bool>(type: "bit", nullable: false),
                    MethodePaiement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dons_Mosquees_MosqueeId",
                        column: x => x.MosqueeId,
                        principalTable: "Mosquees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evenements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MosqueeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evenements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evenements_Mosquees_MosqueeId",
                        column: x => x.MosqueeId,
                        principalTable: "Mosquees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prieres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MosqueeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeureFajr = table.Column<TimeSpan>(type: "time", nullable: true),
                    HeureDuhr = table.Column<TimeSpan>(type: "time", nullable: true),
                    HeureAsr = table.Column<TimeSpan>(type: "time", nullable: true),
                    HeureMaghrib = table.Column<TimeSpan>(type: "time", nullable: true),
                    HeureIsha = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prieres_Mosquees_MosqueeId",
                        column: x => x.MosqueeId,
                        principalTable: "Mosquees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InscriptionsEvenements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EvenementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscriptionsEvenements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscriptionsEvenements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscriptionsEvenements_Evenements_EvenementId",
                        column: x => x.EvenementId,
                        principalTable: "Evenements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BesoinsMateriels_MosqueeId",
                table: "BesoinsMateriels",
                column: "MosqueeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContenusReligieux_UserId",
                table: "ContenusReligieux",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dons_MosqueeId",
                table: "Dons",
                column: "MosqueeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dons_UserId",
                table: "Dons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Evenements_MosqueeId",
                table: "Evenements",
                column: "MosqueeId");

            migrationBuilder.CreateIndex(
                name: "IX_InscriptionsEvenements_EvenementId",
                table: "InscriptionsEvenements",
                column: "EvenementId");

            migrationBuilder.CreateIndex(
                name: "IX_InscriptionsEvenements_UserId",
                table: "InscriptionsEvenements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prieres_MosqueeId",
                table: "Prieres",
                column: "MosqueeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BesoinsMateriels");

            migrationBuilder.DropTable(
                name: "ContenusReligieux");

            migrationBuilder.DropTable(
                name: "Dons");

            migrationBuilder.DropTable(
                name: "InscriptionsEvenements");

            migrationBuilder.DropTable(
                name: "Prieres");

            migrationBuilder.DropTable(
                name: "Evenements");

            migrationBuilder.DropTable(
                name: "Mosquees");
        }
    }
}
