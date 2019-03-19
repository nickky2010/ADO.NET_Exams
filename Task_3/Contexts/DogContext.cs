using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_3.Models;

namespace Task_3.Contexts
{
    class DogContext : DbContext
    {
        public DogContext() : base("DefaultConnection")
        { }
        public DogContext(string connectionString) : base(connectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // использование Fluent API
            modelBuilder.Entity<MyDog>().ToTable("Dogs");
            modelBuilder.Entity<Breed>().Property(p => p.BreedName).HasColumnName("Breed");
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<MyDog> Dogs { get; set; }
        public virtual DbSet<Breed> Breeds { get; set; }
    }
}
