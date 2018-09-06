﻿// <auto-generated />
using Hymperia.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hymperia.Model.Migrations
{
  [DbContext(typeof(DatabaseContext))]
  partial class DatabaseContextModelSnapshot : ModelSnapshot
  {
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "2.2.0-preview1-35029")
          .HasAnnotation("Relational:MaxIdentifierLength", 64);

      modelBuilder.Entity("Hymperia.Model.User", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("Name")
                      .IsRequired()
                      .HasMaxLength(25);

            b.HasKey("Id");

            b.HasAlternateKey("Name");

            b.ToTable("Users");
          });
#pragma warning restore 612, 618
    }
  }
}
