using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academia.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirPersonalIDNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Personals_PersonalID",
                table: "Alunos");

            migrationBuilder.AlterColumn<int>(
                name: "PersonalID",
                table: "Alunos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Personals_PersonalID",
                table: "Alunos",
                column: "PersonalID",
                principalTable: "Personals",
                principalColumn: "PersonalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Personals_PersonalID",
                table: "Alunos");

            migrationBuilder.AlterColumn<int>(
                name: "PersonalID",
                table: "Alunos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Personals_PersonalID",
                table: "Alunos",
                column: "PersonalID",
                principalTable: "Personals",
                principalColumn: "PersonalID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
