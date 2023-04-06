using System;
using ADO.net.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ADO.net.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20230302115248_AddedProducts")]
    partial class AddedProducts
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
#pragma warning restore 612, 618
        }
    }
}
