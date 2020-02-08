using Microsoft.EntityFrameworkCore;
namespace OEE_API.Models.SHW_SHD
{
    public partial class DBContextSHW_SHD : DbContext
    {
        public DBContextSHW_SHD(DbContextOptions<DBContextSHW_SHD> options) : base(options) { }

        public virtual DbSet<ActionTime> ActionTime { get; set; }
        public virtual DbSet<Building_OEE> Building_OEE { get; set; }
        public virtual DbSet<Factory_OEE> Factory_OEE { get; set; }
        public virtual DbSet<Cell_OEE> Cell_OEE { get; set; }
        public virtual DbSet<ShiftTime> ShiftTime { get; set; }
        public virtual DbSet<SHW_OEE_History> SHW_OEE_History { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     base.OnModelCreating(builder);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShiftTime>()
                .HasNoKey();
        }
    }
}
