using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class CollaboratorConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CollaboratorEmail",
                table: "Collaborators",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_CollaboratorEmail_NoteId",
                table: "Collaborators",
                columns: new[] { "CollaboratorEmail", "NoteId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Collaborators_CollaboratorEmail_NoteId",
                table: "Collaborators");

            migrationBuilder.AlterColumn<string>(
                name: "CollaboratorEmail",
                table: "Collaborators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
