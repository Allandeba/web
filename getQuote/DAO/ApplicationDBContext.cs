using System;
using System.Reflection.Metadata;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote.DAO
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

        public DbSet<PersonModel> Person { get; set; }
        public DbSet<DocumentModel> Document { get; set; }
        public DbSet<ContactModel> Contact { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonModel>()
                .Property(p => p.CreationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
        }
    }
}