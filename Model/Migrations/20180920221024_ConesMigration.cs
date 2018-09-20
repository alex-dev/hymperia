using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
    public partial class ConesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrismeRectangulaire_Centre",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Centre",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Point2",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Point1",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Origine",
                table: "Formes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RayonBase",
                table: "Formes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RayonTop",
                table: "Formes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cylindre_ThetaDiv",
                table: "Formes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PrismeRectangulaire_Hauteur",
                table: "Formes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origine",
                table: "Formes");

            migrationBuilder.DropColumn(
                name: "RayonBase",
                table: "Formes");

            migrationBuilder.DropColumn(
                name: "RayonTop",
                table: "Formes");

            migrationBuilder.DropColumn(
                name: "Cylindre_ThetaDiv",
                table: "Formes");

            migrationBuilder.DropColumn(
                name: "PrismeRectangulaire_Hauteur",
                table: "Formes");

            migrationBuilder.AlterColumn<string>(
                name: "PrismeRectangulaire_Centre",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Centre",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Point2",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Point1",
                table: "Formes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
