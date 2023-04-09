using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.Core.Migrations
{
    public partial class payed_installment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PayedInstallment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 4, 9, 14, 25, 20, 634, DateTimeKind.Local).AddTicks(9360));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 4, 9, 14, 25, 20, 634, DateTimeKind.Local).AddTicks(9362));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2023, 4, 9, 14, 25, 20, 634, DateTimeKind.Local).AddTicks(9364));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2023, 4, 9, 14, 25, 20, 634, DateTimeKind.Local).AddTicks(9365));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 5,
                column: "Created",
                value: new DateTime(2023, 4, 9, 14, 25, 20, 634, DateTimeKind.Local).AddTicks(9366));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 6,
                column: "Created",
                value: new DateTime(2023, 4, 9, 14, 25, 20, 634, DateTimeKind.Local).AddTicks(9367));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayedInstallment",
                table: "Order");

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 4, 8, 16, 53, 4, 920, DateTimeKind.Local).AddTicks(1882));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 4, 8, 16, 53, 4, 920, DateTimeKind.Local).AddTicks(1884));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2023, 4, 8, 16, 53, 4, 920, DateTimeKind.Local).AddTicks(1885));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2023, 4, 8, 16, 53, 4, 920, DateTimeKind.Local).AddTicks(1886));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 5,
                column: "Created",
                value: new DateTime(2023, 4, 8, 16, 53, 4, 920, DateTimeKind.Local).AddTicks(1887));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 6,
                column: "Created",
                value: new DateTime(2023, 4, 8, 16, 53, 4, 920, DateTimeKind.Local).AddTicks(1888));
        }
    }
}
