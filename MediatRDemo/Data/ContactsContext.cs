using Microsoft.EntityFrameworkCore;

namespace MediatRDemo.Data
{
    public class ContactsContext : DbContext
    {
        public ContactsContext(DbContextOptions<ContactsContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                        .HasData(new Contact {Id = 1, FirstName = "Steve", LastName = "Michelotti"},
                                 new Contact {Id = 2, FirstName = "Bill", LastName = "Gates"},
                                 new Contact {Id = 3, FirstName = "Satya", LastName = "Nadella"});
        }
    }
}