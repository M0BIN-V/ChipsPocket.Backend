using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChipsPocket.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PictureFileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tables_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    TableId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seat_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TableId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStack_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStack_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChipCollection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserStackId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChipCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChipCollection_UserStack_UserStackId",
                        column: x => x.UserStackId,
                        principalTable: "UserStack",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CollectionChip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChipId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    ChipCollectionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionChip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionChip_ChipCollection_ChipCollectionId",
                        column: x => x.ChipCollectionId,
                        principalTable: "ChipCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionChip_Chip_ChipId",
                        column: x => x.ChipId,
                        principalTable: "Chip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChipCollection_UserStackId",
                table: "ChipCollection",
                column: "UserStackId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionChip_ChipCollectionId",
                table: "CollectionChip",
                column: "ChipCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionChip_ChipId",
                table: "CollectionChip",
                column: "ChipId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_TableId",
                table: "Seat",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_UserId",
                table: "Seat",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_CreatedById",
                table: "Tables",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_Name",
                table: "Tables",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStack_TableId",
                table: "UserStack",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStack_UserId",
                table: "UserStack",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionChip");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "ChipCollection");

            migrationBuilder.DropTable(
                name: "Chip");

            migrationBuilder.DropTable(
                name: "UserStack");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
