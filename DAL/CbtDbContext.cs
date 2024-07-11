using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DemoTask.DAL
{
    public class CbtDbContext : DbContext
    {
        public CbtDbContext(DbContextOptions<CbtDbContext> options)
        : base(options)
        {
        }


        public virtual DbSet<ClientMaster> clientMaster { get; set; }
        public virtual DbSet<OtpMaster> otpMasters { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientMaster>(entity =>
            {
                entity.HasKey(e => e.nId);
            });

            modelBuilder.Entity<OtpMaster>(entity =>
            {
                entity.HasKey(e => e.nId);
            });
        }
    }
}
