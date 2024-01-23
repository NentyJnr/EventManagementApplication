using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NCSEvent.API.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FeedbackEmailSent",
                table: "RegistrationForms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeedbackEmailSent",
                table: "RegistrationForms");
        }
    }
}
