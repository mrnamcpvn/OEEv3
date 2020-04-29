using Microsoft.EntityFrameworkCore;

namespace OEE_API.Models.SHB
{
    public partial class DBContextSHB : DbContext
    {
        public DBContextSHB(DbContextOptions<DBContextSHB> options) : base(options) { }
        public virtual DbSet<Cell_OEE> Cell_OEE { get; set; }
           public virtual DbSet<OeeReport_test> OeeReport_test { get; set; }
        public virtual DbSet<Factory_OEE> Factory_OEE { get; set; }
        public virtual DbSet<SHB_OEE_History> SHB_OEE_History { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
