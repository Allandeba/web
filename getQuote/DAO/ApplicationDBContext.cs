using System.Reflection.Emit;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote.DAO
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<PersonModel> Person { get; set; }
        public DbSet<DocumentModel> Document { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<ItemModel> Item { get; set; }
        public DbSet<ItemImageModel> ItemImage { get; set; }
        public DbSet<ProposalModel> Proposal { get; set; }
        public DbSet<ProposalContentModel> ProposalContent { get; set; }
        public DbSet<ProposalHistoryModel> ProposalHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            PersonInformation(modelBuilder);
            DocumentInformation(modelBuilder);
            ContactInformation(modelBuilder);
            ProposalInformation(modelBuilder);
            ItemInformation(modelBuilder);
        }

        private void PersonInformation(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PersonModel>()
                .Property(p => p.CreationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<PersonModel>()
                .HasOne(PersonModel => PersonModel.Document)
                .WithOne(d => d.Person)
                .HasForeignKey<DocumentModel>(d => d.PersonId);

            modelBuilder
                .Entity<PersonModel>()
                .HasOne(PersonModel => PersonModel.Contact)
                .WithOne(c => c.Person)
                .HasForeignKey<ContactModel>(c => c.PersonId);
        }

        private void DocumentInformation(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DocumentModel>()
                .HasOne(d => d.Person)
                .WithOne(p => p.Document)
                .HasForeignKey<DocumentModel>(d => d.PersonId);
        }

        private void ContactInformation(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ContactModel>()
                .HasOne(d => d.Person)
                .WithOne(p => p.Contact)
                .HasForeignKey<ContactModel>(d => d.PersonId);
        }

        private void ProposalInformation(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ProposalModel>()
                .Property(p => p.ModificationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        }

        private void ItemInformation(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ItemModel>()
                .HasIndex(p => p.ItemName)
                .IsUnique();
        }
    }
}
