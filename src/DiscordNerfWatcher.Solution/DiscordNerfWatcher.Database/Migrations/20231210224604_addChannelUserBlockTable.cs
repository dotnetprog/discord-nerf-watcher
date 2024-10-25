using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordNerfWatcher.Database.Migrations
{
    /// <inheritdoc />
    public partial class addChannelUserBlockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelUserBlocks",
                columns: table => new
                {
                    ChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelUserBlocks", x => new { x.ChannelId, x.UserId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelUserBlocks");
        }
    }
}
