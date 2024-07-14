﻿// <auto-generated />
using System;
using LemonLime.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LemonLime.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240714183204_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("LemonLime.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("LemonLime.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("LemonLime.Models.NutritionInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Calories")
                        .HasColumnType("int");

                    b.Property<float>("Carbohydrates")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("Fat")
                        .HasColumnType("float");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<float>("Protein")
                        .HasColumnType("float");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId")
                        .IsUnique();

                    b.ToTable("NutritionInfos");
                });

            modelBuilder.Entity("LemonLime.Models.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("LemonLime.Models.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("CookingTimeInMinutes")
                        .HasColumnType("int");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("LemonLime.Models.RecipeTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TagId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.HasIndex("TagId");

                    b.ToTable("RecipeTags");
                });

            modelBuilder.Entity("LemonLime.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LemonLime.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("LemonLime.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LemonLime.Models.Comment", b =>
                {
                    b.HasOne("LemonLime.Models.Recipe", "Recipe")
                        .WithMany("Comments")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LemonLime.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LemonLime.Models.Image", b =>
                {
                    b.HasOne("LemonLime.Models.Recipe", "Recipe")
                        .WithMany("Images")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("LemonLime.Models.NutritionInfo", b =>
                {
                    b.HasOne("LemonLime.Models.Recipe", "Recipe")
                        .WithOne("NutritionInfo")
                        .HasForeignKey("LemonLime.Models.NutritionInfo", "RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("LemonLime.Models.Rating", b =>
                {
                    b.HasOne("LemonLime.Models.Recipe", "Recipe")
                        .WithMany("Ratings")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LemonLime.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LemonLime.Models.Recipe", b =>
                {
                    b.HasOne("LemonLime.Models.User", "CreatedByUser")
                        .WithMany("Recipes")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("LemonLime.Models.RecipeTag", b =>
                {
                    b.HasOne("LemonLime.Models.Recipe", "Recipe")
                        .WithMany("RecipeTags")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LemonLime.Models.Tag", "Tag")
                        .WithMany("RecipeTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("LemonLime.Models.User", b =>
                {
                    b.HasOne("LemonLime.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LemonLime.Models.Recipe", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");

                    b.Navigation("NutritionInfo")
                        .IsRequired();

                    b.Navigation("Ratings");

                    b.Navigation("RecipeTags");
                });

            modelBuilder.Entity("LemonLime.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LemonLime.Models.Tag", b =>
                {
                    b.Navigation("RecipeTags");
                });

            modelBuilder.Entity("LemonLime.Models.User", b =>
                {
                    b.Navigation("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
