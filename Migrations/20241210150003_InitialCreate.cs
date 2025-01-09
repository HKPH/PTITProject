using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookCategory",
                columns: table => new
                {
                    bookId = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3213E83FBA9C5017", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    paymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    paymentMethod = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__3213E83FD08F1548", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Publishe__3213E83F4BB6DE63", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    dob = table.Column<DateTime>(type: "datetime", nullable: true),
                    gender = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3213E83FD52EBEA5", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: true),
                    author = table.Column<string>(type: "longtext", nullable: true),
                    publisherId = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    publicationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    stockQuantity = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true),
                    image = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Book__3213E83FCA72EFC3", x => x.id);
                    table.ForeignKey(
                        name: "FK__Book__publisherI__498EEC8D",
                        column: x => x.publisherId,
                        principalTable: "Publisher",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    role = table.Column<int>(type: "int", nullable: true),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3213E83F2B6CCC80", x => x.id);
                    table.ForeignKey(
                        name: "FK_Account_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__3213E83F269FFE73", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cart_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShippingAddress",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    note = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    customerNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipping__3213E83F44594E3C", x => x.id);
                    table.ForeignKey(
                        name: "FK_ShippingAddress_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JoinBookCategory",
                columns: table => new
                {
                    bookId = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__JoinBook__19D90E10B4823A18", x => new { x.bookId, x.categoryId });
                    table.ForeignKey(
                        name: "FK_JoinBookCategory_Book",
                        column: x => x.bookId,
                        principalTable: "Book",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_JoinBookCategory_Category",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    bookId = table.Column<int>(type: "int", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true),
                    value = table.Column<int>(type: "int", nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    comment = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    photo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rating__3213E83FF89B23AB", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rating_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Rating__bookId__55F4C372",
                        column: x => x.bookId,
                        principalTable: "Book",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__3214EC07D80B5D6C", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItem_Book",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CartItem_Cart",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    shippingAddressId = table.Column<int>(type: "int", nullable: true),
                    fee = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    dateReceived = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipment__3213E83F2F191BEA", x => x.id);
                    table.ForeignKey(
                        name: "FK__Shipment__shippi__671F4F74",
                        column: x => x.shippingAddressId,
                        principalTable: "ShippingAddress",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(type: "int", nullable: false),
                    orderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    shipmentId = table.Column<int>(type: "int", nullable: true),
                    paymentId = table.Column<int>(type: "int", nullable: true),
                    totalPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3213E83F7E266E2C", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_Payment",
                        column: x => x.paymentId,
                        principalTable: "Payment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Order_Shipment",
                        column: x => x.shipmentId,
                        principalTable: "Shipment",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderIte__3214EC07F2EFED56", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Account_userId",
                table: "Account",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_publisherId",
                table: "Book",
                column: "publisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_userId",
                table: "Cart",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_BookId",
                table: "CartItem",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinBookCategory_categoryId",
                table: "JoinBookCategory",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_paymentId",
                table: "Order",
                column: "paymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_shipmentId",
                table: "Order",
                column: "shipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_BookId",
                table: "OrderItem",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_bookId",
                table: "Rating",
                column: "bookId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_userId",
                table: "Rating",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_shippingAddressId",
                table: "Shipment",
                column: "shippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_userId",
                table: "ShippingAddress",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "BookCategory");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "JoinBookCategory");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropTable(
                name: "ShippingAddress");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
