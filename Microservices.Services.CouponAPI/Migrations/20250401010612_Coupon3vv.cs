using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class Coupon3vv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "MinimumAmount",
                value: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "MinimumAmount",
                value: 10);
        }
    }
}
