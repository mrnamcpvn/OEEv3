using Microsoft.EntityFrameworkCore;

namespace OEE_API.Models.SYF
{
    public partial class DBContextSYF : DbContext
    {
        public DBContextSYF(DbContextOptions<DBContextSYF> options) : base(options) { }
        public virtual DbSet<ActionTime> ActionTime { get; set; }
        public virtual DbSet<Building_OEE> Building_OEE { get; set; }
        public virtual DbSet<Cell_OEE> Cell_OEE { get; set; }
        public virtual DbSet<Factory_OEE> Factory_OEE { get; set; }
        public virtual DbSet<SY2_OEE_History> SY2_OEE_History { get; set; }

        public virtual DbSet<OeeReport_test> OeeReport_test {get;set;}

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionTime>()
                .HasNoKey();
        }
    }
}
