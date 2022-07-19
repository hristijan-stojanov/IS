using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KinoBileti.Data.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ime",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prezime",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uloga",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bilets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ime = table.Column<string>(nullable: true),
                    datum = table.Column<DateTime>(nullable: false),
                    cena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ownerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCards_AspNetUsers_ownerId",
                        column: x => x.ownerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BiletInShoppings",
                columns: table => new
                {
                    BiletId = table.Column<Guid>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiletInShoppings", x => new { x.ShoppingCartId, x.BiletId });
                    table.ForeignKey(
                        name: "FK_BiletInShoppings_ShoppingCards_BiletId",
                        column: x => x.BiletId,
                        principalTable: "ShoppingCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiletInShoppings_Bilets_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "Bilets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiletInShoppings_BiletId",
                table: "BiletInShoppings",
                column: "BiletId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCards_ownerId",
                table: "ShoppingCards",
                column: "ownerId",
                unique: true,
                filter: "[ownerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiletInShoppings");

            migrationBuilder.DropTable(
                name: "ShoppingCards");

            migrationBuilder.DropTable(
                name: "Bilets");

            migrationBuilder.DropColumn(
                name: "ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "prezime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "uloga",
                table: "AspNetUsers");
        }
    }
}
