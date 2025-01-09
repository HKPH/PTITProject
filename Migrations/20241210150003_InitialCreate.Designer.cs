﻿// <auto-generated />
using System;
using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookStore.Migrations
{
    [DbContext(typeof(BookStoreContext))]
    [Migration("20241210150003_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BookStore.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("active");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("createDate");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<int?>("Role")
                        .HasColumnType("int")
                        .HasColumnName("role");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("PK__Account__3213E83F2B6CCC80");

                    b.HasIndex("UserId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("active");

                    b.Property<string>("Author")
                        .HasColumnType("longtext")
                        .HasColumnName("author");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .HasColumnType("longtext")
                        .HasColumnName("image");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18, 3)")
                        .HasColumnName("price");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("publicationDate");

                    b.Property<int?>("PublisherId")
                        .HasColumnType("int")
                        .HasColumnName("publisherId");

                    b.Property<int?>("StockQuantity")
                        .HasColumnType("int")
                        .HasColumnName("stockQuantity");

                    b.Property<string>("Title")
                        .HasColumnType("longtext")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PK__Book__3213E83FCA72EFC3");

                    b.HasIndex("PublisherId");

                    b.ToTable("Book", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.BookCategory", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int")
                        .HasColumnName("bookId");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("categoryId");

                    b.ToTable("BookCategory", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("PK__Cart__3213E83F269FFE73");

                    b.HasIndex("UserId");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__CartItem__3214EC07D80B5D6C");

                    b.HasIndex("BookId");

                    b.HasIndex("CartId");

                    b.ToTable("CartItem", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("active");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__Category__3213E83FBA9C5017");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime")
                        .HasColumnName("orderDate");

                    b.Property<int?>("PaymentId")
                        .HasColumnType("int")
                        .HasColumnName("paymentId");

                    b.Property<int?>("ShipmentId")
                        .HasColumnType("int")
                        .HasColumnName("shipmentId");

                    b.Property<int?>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18, 3)")
                        .HasColumnName("totalPrice");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("PK__Order__3213E83F7E266E2C");

                    b.HasIndex("PaymentId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18, 3)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__OrderIte__3214EC07F2EFED56");

                    b.HasIndex("BookId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItem", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18, 3)")
                        .HasColumnName("amount");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime")
                        .HasColumnName("paymentDate");

                    b.Property<string>("PaymentMethod")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("paymentMethod");

                    b.HasKey("Id")
                        .HasName("PK__Payment__3213E83FD08F1548");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("active");

                    b.Property<string>("Name")
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__Publishe__3213E83F4BB6DE63");

                    b.ToTable("Publisher", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("active");

                    b.Property<int?>("BookId")
                        .HasColumnType("int")
                        .HasColumnName("bookId");

                    b.Property<string>("Comment")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("comment");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("createDate");

                    b.Property<string>("Photo")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("photo");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.Property<int?>("Value")
                        .HasColumnType("int")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("PK__Rating__3213E83FF89B23AB");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Rating", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Shipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("DateReceived")
                        .HasColumnType("datetime")
                        .HasColumnName("dateReceived");

                    b.Property<decimal?>("Fee")
                        .HasColumnType("decimal(18, 3)")
                        .HasColumnName("fee");

                    b.Property<int?>("ShippingAddressId")
                        .HasColumnType("int")
                        .HasColumnName("shippingAddressId");

                    b.HasKey("Id")
                        .HasName("PK__Shipment__3213E83F2F191BEA");

                    b.HasIndex("ShippingAddressId");

                    b.ToTable("Shipment", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.ShippingAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("address");

                    b.Property<string>("CustomerNumber")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("customerNumber");

                    b.Property<string>("Note")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("note");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("PK__Shipping__3213E83F44594E3C");

                    b.HasIndex("UserId");

                    b.ToTable("ShippingAddress", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("active");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("address");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime")
                        .HasColumnName("dob");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<int?>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("gender");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("phone");

                    b.HasKey("Id")
                        .HasName("PK__User__3213E83FD52EBEA5");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("JoinBookCategory", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int")
                        .HasColumnName("bookId");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("categoryId");

                    b.HasKey("BookId", "CategoryId")
                        .HasName("PK__JoinBook__19D90E10B4823A18");

                    b.HasIndex("CategoryId");

                    b.ToTable("JoinBookCategory", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Account", b =>
                {
                    b.HasOne("BookStore.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Account_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookStore.Models.Book", b =>
                {
                    b.HasOne("BookStore.Models.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .HasConstraintName("FK__Book__publisherI__498EEC8D");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("BookStore.Models.Cart", b =>
                {
                    b.HasOne("BookStore.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Cart_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookStore.Models.CartItem", b =>
                {
                    b.HasOne("BookStore.Models.Book", "Book")
                        .WithMany("CartItems")
                        .HasForeignKey("BookId")
                        .IsRequired()
                        .HasConstraintName("FK_CartItem_Book");

                    b.HasOne("BookStore.Models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .IsRequired()
                        .HasConstraintName("FK_CartItem_Cart");

                    b.Navigation("Book");

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("BookStore.Models.Order", b =>
                {
                    b.HasOne("BookStore.Models.Payment", "Payment")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentId")
                        .HasConstraintName("FK_Order_Payment");

                    b.HasOne("BookStore.Models.Shipment", "Shipment")
                        .WithMany("Orders")
                        .HasForeignKey("ShipmentId")
                        .HasConstraintName("FK_Order_Shipment");

                    b.Navigation("Payment");

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("BookStore.Models.OrderItem", b =>
                {
                    b.HasOne("BookStore.Models.Book", "Book")
                        .WithMany("OrderItems")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookStore.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BookStore.Models.Rating", b =>
                {
                    b.HasOne("BookStore.Models.Book", "Book")
                        .WithMany("Ratings")
                        .HasForeignKey("BookId")
                        .HasConstraintName("FK__Rating__bookId__55F4C372");

                    b.HasOne("BookStore.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Rating_User");

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookStore.Models.Shipment", b =>
                {
                    b.HasOne("BookStore.Models.ShippingAddress", "ShippingAddress")
                        .WithMany("Shipments")
                        .HasForeignKey("ShippingAddressId")
                        .HasConstraintName("FK__Shipment__shippi__671F4F74");

                    b.Navigation("ShippingAddress");
                });

            modelBuilder.Entity("BookStore.Models.ShippingAddress", b =>
                {
                    b.HasOne("BookStore.Models.User", "User")
                        .WithMany("ShippingAddresses")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_ShippingAddress_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("JoinBookCategory", b =>
                {
                    b.HasOne("BookStore.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BookId")
                        .IsRequired()
                        .HasConstraintName("FK_JoinBookCategory_Book");

                    b.HasOne("BookStore.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_JoinBookCategory_Category");
                });

            modelBuilder.Entity("BookStore.Models.Book", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("OrderItems");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("BookStore.Models.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("BookStore.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BookStore.Models.Payment", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("BookStore.Models.Publisher", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookStore.Models.Shipment", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("BookStore.Models.ShippingAddress", b =>
                {
                    b.Navigation("Shipments");
                });

            modelBuilder.Entity("BookStore.Models.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Carts");

                    b.Navigation("Ratings");

                    b.Navigation("ShippingAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}
