using Microsoft.EntityFrameworkCore.Migrations;

namespace ESMS.Data.Migrations
{
    public partial class NewFieldsForUSer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Country",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true, 
                defaultValue: 1);


            migrationBuilder.AlterColumn<int>(
                name: "City",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                defaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IbanCode",
                table: "AspNetUsers",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IbanCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Country",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "City",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
