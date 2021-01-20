using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace aiof.messaging.data
{
    public class MessagingContext : DbContext
    {
        public virtual DbSet<TestConfig> TestConfigs { get; set; }

        public MessagingContext(DbContextOptions<MessagingContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("msg");

            modelBuilder.Entity<TestConfig>(e =>
            {
                e.ToTable(Entity.TestConfig);

                e.HasKey(x => x.Id);

                e.Property(x => x.Id).ValueGeneratedOnAdd().HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.PublicKey).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.Type).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.Email).HasSnakeCaseColumnName();
                e.Property(x => x.PhoneNumber).HasSnakeCaseColumnName();
            });
        }
    }
}
