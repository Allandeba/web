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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Person Information
            modelBuilder
                .Entity<PersonModel>()
                .Property(p => p.CreationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            //Proposal information
            modelBuilder
                .Entity<ProposalModel>()
                .Property(p => p.ModificationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
