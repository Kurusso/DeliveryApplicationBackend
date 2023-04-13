﻿// <auto-generated />
using System;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryAgreagatorBackendApplication.Migrations
{
    [DbContext(typeof(BackendDbContext))]
    [Migration("20230326114224_addDishInCart")]
    partial class addDishInCart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.CookDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Cooks");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.CourierDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.CustomerDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.DishDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsVegetarian")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.DishInCartDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<int>("Counter")
                        .HasColumnType("integer");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.ToTable("DishInCart");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.MenuDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.OrderDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("CookId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DeliveryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CookId");

                    b.HasIndex("CourierId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.RatingDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DishId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.RestaurantDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("DishDbModelMenuDbModel", b =>
                {
                    b.Property<Guid>("DishesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MenusId")
                        .HasColumnType("uuid");

                    b.HasKey("DishesId", "MenusId");

                    b.HasIndex("MenusId");

                    b.ToTable("DishDbModelMenuDbModel");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.CookDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.RestaurantDbModel", "Restaurant")
                        .WithMany("Cooks")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.DishDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.RestaurantDbModel", "Restaurant")
                        .WithMany()
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.DishInCartDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.CustomerDbModel", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.DishDbModel", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.OrderDbModel", "Order")
                        .WithMany("DishesInCart")
                        .HasForeignKey("OrderId");

                    b.Navigation("Customer");

                    b.Navigation("Dish");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.MenuDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.RestaurantDbModel", "Restaurant")
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.OrderDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.CookDbModel", "Cook")
                        .WithMany()
                        .HasForeignKey("CookId");

                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.CourierDbModel", "Courier")
                        .WithMany()
                        .HasForeignKey("CourierId");

                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.CustomerDbModel", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cook");

                    b.Navigation("Courier");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.RatingDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.CustomerDbModel", "Customer")
                        .WithMany("Ratings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.DishDbModel", "Dish")
                        .WithMany("Ratings")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("DishDbModelMenuDbModel", b =>
                {
                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.DishDbModel", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryAgreagatorBackendApplication.Models.MenuDbModel", null)
                        .WithMany()
                        .HasForeignKey("MenusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.CustomerDbModel", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.DishDbModel", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.OrderDbModel", b =>
                {
                    b.Navigation("DishesInCart");
                });

            modelBuilder.Entity("DeliveryAgreagatorBackendApplication.Models.RestaurantDbModel", b =>
                {
                    b.Navigation("Cooks");

                    b.Navigation("Menus");
                });
#pragma warning restore 612, 618
        }
    }
}
