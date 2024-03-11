using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddJiraIdToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AddColumn<string>(
                name: "JiraId",
                table: "Tasks",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE Tasks SET Title=SUBSTRING(t.Title, POSITION('[' IN t.Title) + 1, POSITION(']' IN t.Title) - 2) FROM Tasks AS t  WHERE POSITION('[' IN t.Title) > 0 AND POSITION(']' IN t.Title) > 0");

            migrationBuilder.Sql(
                "UPDATE Tasks SET Title=val FROM SUBSTRING(t.Title, POSITION(']' IN t.Title) + 1) AS val FROM Tasks AS t WHERE Tasks.JiraId NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JiraId",
                table: "Tasks");
        }
    }
}
