using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcanumLogic.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    telegram_id = table.Column<long>(type: "INTEGER", nullable: false),
                    character_name = table.Column<string>(type: "TEXT", nullable: false),
                    telegram_name = table.Column<string>(type: "TEXT", nullable: false),
                    wallet_code = table.Column<string>(type: "TEXT", nullable: true),
                    destiny_points_start = table.Column<string>(type: "TEXT", nullable: false),
                    magic_points_start = table.Column<string>(type: "TEXT", nullable: false),
                    tech_points_start = table.Column<string>(type: "TEXT", nullable: false),
                    is_vip = table.Column<string>(type: "TEXT", nullable: false),
                    zones_text = table.Column<string>(type: "TEXT", nullable: false),
                    scoring = table.Column<string>(type: "TEXT", nullable: false),
                    fabrica_searchkey = table.Column<string>(type: "TEXT", nullable: false),
                    exp = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "bids",
                columns: table => new
                {
                    bid_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<int>(type: "INTEGER", nullable: false),
                    account_id = table.Column<long>(type: "INTEGER", nullable: false),
                    imagine_id = table.Column<long>(type: "INTEGER", nullable: false),
                    payed = table.Column<decimal>(type: "TEXT", nullable: false),
                    cycle = table.Column<int>(type: "INTEGER", nullable: false),
                    currency = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bids", x => x.bid_id);
                });

            migrationBuilder.CreateTable(
                name: "fabricas",
                columns: table => new
                {
                    fabrica_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    search_key = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    current_max = table.Column<string>(type: "TEXT", nullable: false),
                    progress_per_one = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fabricas", x => x.fabrica_id);
                });

            migrationBuilder.CreateTable(
                name: "imagines",
                columns: table => new
                {
                    imagine_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    search_key = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    currency = table.Column<string>(type: "TEXT", nullable: false),
                    value_start = table.Column<string>(type: "TEXT", nullable: false),
                    magic_value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagines", x => x.imagine_id);
                });

            migrationBuilder.CreateTable(
                name: "transfers",
                columns: table => new
                {
                    transfer_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    account_id = table.Column<long>(type: "INTEGER", nullable: false),
                    comment = table.Column<string>(type: "TEXT", nullable: false),
                    currency = table.Column<string>(type: "TEXT", nullable: false),
                    transfer_time = table.Column<string>(type: "TEXT", nullable: false),
                    currency_value = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfers", x => x.transfer_id);
                });

            migrationBuilder.CreateTable(
                name: "trees",
                columns: table => new
                {
                    tree_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    search_key = table.Column<string>(type: "TEXT", nullable: false),
                    currency = table.Column<string>(type: "TEXT", nullable: false),
                    cost = table.Column<string>(type: "TEXT", nullable: false),
                    parent_tree = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trees", x => x.tree_id);
                    table.UniqueConstraint("AK_trees_search_key", x => x.search_key);
                });

            migrationBuilder.CreateTable(
                name: "research",
                columns: table => new
                {
                    research_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    account_id = table.Column<long>(type: "INTEGER", nullable: true),
                    search_key = table.Column<string>(type: "TEXT", nullable: false),
                    time_of_research = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_research", x => x.research_id);
                    table.ForeignKey(
                        name: "FK_research_trees_search_key",
                        column: x => x.search_key,
                        principalTable: "trees",
                        principalColumn: "search_key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_research_search_key",
                table: "research",
                column: "search_key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "bids");

            migrationBuilder.DropTable(
                name: "fabricas");

            migrationBuilder.DropTable(
                name: "imagines");

            migrationBuilder.DropTable(
                name: "research");

            migrationBuilder.DropTable(
                name: "transfers");

            migrationBuilder.DropTable(
                name: "trees");
        }
    }
}
