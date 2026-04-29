using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailsAndLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$GGnSniY13qp3ciEwdQ67vuWCb1BPn6B2r2SAVKSKFfG0gl3wdiGW6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$UHU5.NceGVUaqv02QDTEnOxGMIsfUOsg.Fb9grGf1IBsMsuslo7i2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$HKruOAbso0ogBJQNlspMzeUh5IU0oULSG7vfQ5ygtN72T5p7olOpG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LkfJTaJC.5Rc/f71MkE4nO7JNnjC8nj1LlnHasftQ4CHVhl53agVy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$sfjSG/nNtx.InDy.gx63beBJiU.eR8t4ib00Y5SyQ1Jr9LnseYLKi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$89rqjRPvYUfM0RLymF.WbOZnE8PO1f8vvv4y.baaXH..Kj4QRyoZK");
        }
    }
}
