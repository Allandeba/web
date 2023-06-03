﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using getQuote.DAO;

#nullable disable

namespace getQuote.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("getQuote.Models.ContactModel", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("ContactId");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("getQuote.Models.DocumentModel", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Document");
                });

            modelBuilder.Entity("getQuote.Models.PersonModel", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PersonId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("getQuote.Models.ContactModel", b =>
                {
                    b.HasOne("getQuote.Models.PersonModel", "Person")
                        .WithOne("Contact")
                        .HasForeignKey("getQuote.Models.ContactModel", "PersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("getQuote.Models.DocumentModel", b =>
                {
                    b.HasOne("getQuote.Models.PersonModel", "Person")
                        .WithOne("Document")
                        .HasForeignKey("getQuote.Models.DocumentModel", "PersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("getQuote.Models.PersonModel", b =>
                {
                    b.Navigation("Contact")
                        .IsRequired();

                    b.Navigation("Document")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
