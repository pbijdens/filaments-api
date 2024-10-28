using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace FilamentsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedFilamentsAndStorageboxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Storageboxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: false),
                    LastDessicantChange = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    PhotoID = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storageboxes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Filaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: false),
                    Brand = table.Column<string>(type: "longtext", nullable: false),
                    Kind = table.Column<string>(type: "longtext", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    PricePerKG = table.Column<double>(type: "double", nullable: false),
                    InitialWeight = table.Column<int>(type: "int", nullable: false),
                    FirstAdded = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    Color1 = table.Column<string>(type: "longtext", nullable: false),
                    Color2 = table.Column<string>(type: "longtext", nullable: false),
                    PhotoID = table.Column<string>(type: "longtext", nullable: false),
                    StorageBoxId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filaments_Storageboxes_StorageBoxId",
                        column: x => x.StorageBoxId,
                        principalTable: "Storageboxes",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Filaments_StorageBoxId",
                table: "Filaments",
                column: "StorageBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filaments");

            migrationBuilder.DropTable(
                name: "Storageboxes");
        }
    }
}
