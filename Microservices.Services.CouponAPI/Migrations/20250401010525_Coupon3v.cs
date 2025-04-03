using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class Coupon3v : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "MinimumAmount",
                value: 40);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "MinimumAmount",
                value: 200);
        }
    }
}
