﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sprint_sol1.Data;

#nullable disable

namespace Sprint_sol1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240911050728_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sprint_sol1.Models.Department", b =>
                {
                    b.Property<int>("Dept_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Dept_ID"));

                    b.Property<string>("Dept_Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Dept_ID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Sprint_sol1.Models.Employee", b =>
                {
                    b.Property<string>("Emp_ID")
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("Emp_Basic")
                        .HasColumnType("int");

                    b.Property<string>("Emp_Contact_Num")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("Emp_Date_of_Birth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Emp_Date_of_Joining")
                        .HasColumnType("datetime2");

                    b.Property<int>("Emp_Dept_ID")
                        .HasColumnType("int");

                    b.Property<string>("Emp_Designation")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Emp_First_Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Emp_Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emp_Grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Emp_Home_Address")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Emp_Last_Name")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Emp_Marital_Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Emp_ID");

                    b.HasIndex("Emp_Dept_ID");

                    b.HasIndex("Emp_Grade");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Sprint_sol1.Models.GradeMaster", b =>
                {
                    b.Property<string>("Grade_Code")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Max_Salary")
                        .HasColumnType("int");

                    b.Property<int>("Min_Salary")
                        .HasColumnType("int");

                    b.HasKey("Grade_Code");

                    b.ToTable("GradeMasters");
                });

            modelBuilder.Entity("Sprint_sol1.Models.UserMaster", b =>
                {
                    b.Property<string>("UserID")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("UserID");

                    b.ToTable("UserMasters");
                });

            modelBuilder.Entity("Sprint_sol1.Models.Employee", b =>
                {
                    b.HasOne("Sprint_sol1.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("Emp_Dept_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sprint_sol1.Models.GradeMaster", "GradeMaster")
                        .WithMany()
                        .HasForeignKey("Emp_Grade")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sprint_sol1.Models.UserMaster", "UserMaster")
                        .WithMany()
                        .HasForeignKey("Emp_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("GradeMaster");

                    b.Navigation("UserMaster");
                });
#pragma warning restore 612, 618
        }
    }
}
