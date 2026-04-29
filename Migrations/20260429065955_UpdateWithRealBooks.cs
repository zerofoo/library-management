using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWithRealBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$iH5YB9NoY7WojumupQaaZuPrRxZcZfkDO2tQ.KU5yk2JXcDl4ZJ76");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$26strlxj2B5fpNpt885DyO3mu6/sTFF1Mf4OzCFcbP1uzdDn0MCxe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$MErRrJwU90quVk4OyLZJt.KSwHMCDME6nYePa6NG6AyBq2mwuS19m");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$tYtXEFcnACJ5TyKpQSmlS.tRhL7hGZBWsdRxaChySXWx3LphSknFO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$0iI2Uj1ONG6Gi/yU.0TbOuJ6tKj6hf./.UoSaeKlLmRB9KA4tTEOW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$ylz9zJ2U0M68OMxpbhum3Ou334wRsbmFZxxC6Hr0e4jl11agSeIEq");
        }
    }
}
