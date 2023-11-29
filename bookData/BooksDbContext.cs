using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookData
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext() : base("name=BooksConnection")
        {
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.Id); // Defines 'Id' as the primary key
                                                           // Other configurations...
        }

    }

}
