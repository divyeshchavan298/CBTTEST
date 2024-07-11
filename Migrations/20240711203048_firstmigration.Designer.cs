﻿// <auto-generated />
using System;
using DemoTask.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoTask.Migrations
{
    [DbContext(typeof(CbtDbContext))]
    [Migration("20240711203048_firstmigration")]
    partial class firstmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DemoTask.DAL.ClientMaster", b =>
                {
                    b.Property<int>("nId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("bPrivacyPolicy")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("dtCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("dtLastUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("sBiometric")
                        .HasColumnType("longtext");

                    b.Property<string>("sCustomerName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sIcNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sMobileNo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sPin")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("nId");

                    b.ToTable("clientMaster");
                });

            modelBuilder.Entity("DemoTask.DAL.OtpMaster", b =>
                {
                    b.Property<int>("nId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("bOtpVerify")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("dtOtpGentnTime")
                        .HasColumnType("datetime(6)");

                    b.Property<short>("nOtpType")
                        .HasColumnType("smallint");

                    b.Property<short>("nResendOTPCount")
                        .HasColumnType("smallint");

                    b.Property<string>("sIcNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sOtp")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sOtpFor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("nId");

                    b.ToTable("otpMasters");
                });
#pragma warning restore 612, 618
        }
    }
}
