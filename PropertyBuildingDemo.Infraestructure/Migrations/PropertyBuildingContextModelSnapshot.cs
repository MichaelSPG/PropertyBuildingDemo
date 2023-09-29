﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PropertyBuildingDemo.Infrastructure.Data;

#nullable disable

namespace PropertyBuildingDemo.Infrastructure.Migrations
{
    [DbContext(typeof(PropertyBuildingContext))]
    partial class PropertyBuildingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.Owner", b =>
                {
                    b.Property<long>("IdOwner")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IdOwner"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IdOwner");

                    b.ToTable("Owner");
                });

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.Property", b =>
                {
                    b.Property<long>("IdProperty")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IdProperty"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("IdOwner")
                        .HasColumnType("bigint");

                    b.Property<string>("InternalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<short>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("IdProperty");

                    b.HasIndex("IdOwner");

                    b.ToTable("Property");
                });

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.PropertyImage", b =>
                {
                    b.Property<long>("IdPropertyImage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IdPropertyImage"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<long>("IdProperty")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IdPropertyImage");

                    b.HasIndex("IdProperty");

                    b.ToTable("PropertyImage");
                });

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.PropertyTrace", b =>
                {
                    b.Property<long>("IdPropertyTrace")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IdPropertyTrace"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSale")
                        .HasColumnType("datetime2");

                    b.Property<long>("IdProperty")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Tax")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Value")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPropertyTrace");

                    b.HasIndex("IdProperty");

                    b.ToTable("PropertyTrace");
                });

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.Property", b =>
                {
                    b.HasOne("PropertyBuildingDemo.Domain.Entities.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("IdOwner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.PropertyImage", b =>
                {
                    b.HasOne("PropertyBuildingDemo.Domain.Entities.Property", "Property")
                        .WithMany()
                        .HasForeignKey("IdProperty")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });

            modelBuilder.Entity("PropertyBuildingDemo.Domain.Entities.PropertyTrace", b =>
                {
                    b.HasOne("PropertyBuildingDemo.Domain.Entities.Property", "Property")
                        .WithMany()
                        .HasForeignKey("IdProperty")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });
#pragma warning restore 612, 618
        }
    }
}
