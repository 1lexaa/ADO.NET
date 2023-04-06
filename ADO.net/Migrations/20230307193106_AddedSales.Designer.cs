using System;
using ADO.net.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ADO.net.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20230307193106_AddedSales")]
    partial class AddedSales
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ADO.NET.EFCore.Department", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Departments");
            });

            modelBuilder.Entity("ADO.NET.EFCore.Manager", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("FiredDt")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("IdChief")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("IdMainDep")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("IdSecDep")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Secname")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Surname")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Managers");
            });

            modelBuilder.Entity("ADO.NET.EFCore.Product", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("DeleteData")
                    .HasColumnType("datetime2");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<double>("Price")
                    .HasColumnType("float");

                b.HasKey("Id");

                b.ToTable("Products");
            });

            modelBuilder.Entity("ADO.NET.EFCore.Sale", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("Cnt")
                    .HasColumnType("int");

                b.Property<DateTime?>("DeleteDt")
                    .HasColumnType("datetime2");

                b.Property<Guid>("IdManager")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("IdProduct")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("SaleDt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.ToTable("Sales");
            });
#pragma warning restore 612, 618
        }
    }
}