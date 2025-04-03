using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class Coupon3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "MinimumAmount",
                value: 10);

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount", "MinimumAmount" },
                values: new object[] { 3, "30OFF", 30.0, 60 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "MinimumAmount",
                value: 100);
        }
    }
}
