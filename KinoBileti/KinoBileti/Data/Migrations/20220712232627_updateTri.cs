using Microsoft.EntityFrameworkCore.Migrations;

namespace KinoBileti.Data.Migrations
{
    public partial class updateTri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiletInOrder_Bilets_BiletId",
                table: "BiletInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BiletInOrder_Order_OrderId",
                table: "BiletInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_userId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BiletInOrder",
                table: "BiletInOrder");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "BiletInOrder",
                newName: "BiletInOrders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_userId",
                table: "Orders",
                newName: "IX_Orders_userId");

            migrationBuilder.RenameIndex(
                name: "IX_BiletInOrder_OrderId",
                table: "BiletInOrders",
                newName: "IX_BiletInOrders_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BiletInOrders",
                table: "BiletInOrders",
                columns: new[] { "BiletId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BiletInOrders_Bilets_BiletId",
                table: "BiletInOrders",
                column: "BiletId",
                principalTable: "Bilets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BiletInOrders_Orders_OrderId",
                table: "BiletInOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_userId",
                table: "Orders",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiletInOrders_Bilets_BiletId",
                table: "BiletInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BiletInOrders_Orders_OrderId",
                table: "BiletInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_userId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BiletInOrders",
                table: "BiletInOrders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "BiletInOrders",
                newName: "BiletInOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_userId",
                table: "Order",
                newName: "IX_Order_userId");

            migrationBuilder.RenameIndex(
                name: "IX_BiletInOrders_OrderId",
                table: "BiletInOrder",
                newName: "IX_BiletInOrder_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BiletInOrder",
                table: "BiletInOrder",
                columns: new[] { "BiletId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BiletInOrder_Bilets_BiletId",
                table: "BiletInOrder",
                column: "BiletId",
                principalTable: "Bilets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BiletInOrder_Order_OrderId",
                table: "BiletInOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_userId",
                table: "Order",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
