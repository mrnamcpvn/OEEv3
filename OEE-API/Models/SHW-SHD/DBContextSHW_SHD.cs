using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace OEE_API.Models.SHW_SHD
{
    public partial class DBContextSHW_SHD : DbContext
    {
        public DBContextSHW_SHD(DbContextOptions<DBContextSHW_SHD> options) : base(options) { }

        public virtual DbSet<ActionTime> ActionTime { get; set; }
        public virtual DbSet<Building_OEE> Building_OEE { get; set; }
        public virtual DbSet<Cell_OEE> Cell_OEE { get; set; }
        public virtual DbSet<DowntimeDetail> DowntimeDetail { get; set; }
        public virtual DbSet<DowntimeReason> DowntimeReason { get; set; }
        public virtual DbSet<Factory_OEE> Factory_OEE { get; set; }
        public virtual DbSet<MaintenanceTime> MaintenanceTime { get; set; }
        public virtual DbSet<OeeReport_test> OeeReport_test { get; set; }
        public virtual DbSet<OeeReport_Today> OeeReport_Today { get; set; }
        public virtual DbSet<RestTime> RestTime { get; set; }
        public virtual DbSet<SHW_OEE_History> SHW_OEE_History { get; set; }
        public virtual DbSet<ShiftTime> ShiftTime { get; set; }
                public virtual DbSet<MachineInformation> MachineInformation {get;set;}
        public virtual DbSet<RowsAffected> Row {get;set;}

        public virtual  DbSet<M_Users> M_Users {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionTime>(entity =>
            {
                entity.HasKey(x => x.id);
            });

            modelBuilder.Entity<Building_OEE>(entity =>
            {
                entity.HasKey(x => x.id);

            });

            modelBuilder.Entity<Cell_OEE>(entity =>
            {
                entity.HasKey(x => x.id);

            });

            modelBuilder.Entity<DowntimeDetail>(entity =>
            {
                entity.HasKey(x => x.id);
            });

            modelBuilder.Entity<DowntimeReason>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Factory_OEE>(entity =>
            {
                entity.Property(e => e.Factory).IsUnicode(false);
            });

            modelBuilder.Entity<MaintenanceTime>(entity =>
            {
                entity.Property(e => e.machine_id).IsUnicode(false);
            });

            modelBuilder.Entity<OeeReport_test>(entity =>
            {
                entity.Property(e => e.Building).IsUnicode(false);

                entity.Property(e => e.Factory).IsUnicode(false);

                entity.Property(e => e.Machine).IsUnicode(false);

                entity.Property(e => e.Remark)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Shift_ID)
                    .IsUnicode(false)
                    .IsFixedLength();
            });
            modelBuilder.Entity<OeeReport_Today>(entity =>
                     {
                         entity.Property(e => e.Building).IsUnicode(false);

                         entity.Property(e => e.Factory).IsUnicode(false);

                         entity.Property(e => e.Machine).IsUnicode(false);

                         entity.Property(e => e.Remark)
                             .IsUnicode(false)
                             .IsFixedLength();

                         entity.Property(e => e.Shift_ID)
                             .IsUnicode(false)
                             .IsFixedLength();
                     });
            modelBuilder.Entity<RestTime>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.shift_id, e.building_id });
            });

            modelBuilder.Entity<SHW_OEE_History>(entity =>
            {
                entity.Property(e => e.Building).IsUnicode(false);

                entity.Property(e => e.Downtime).IsUnicode(false);

                entity.Property(e => e.Factory).IsUnicode(false);

                entity.Property(e => e.Machine).IsUnicode(false);
            });

            modelBuilder.Entity<ShiftTime>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.shift_id, e.building_id });
            });
            modelBuilder.Entity<RowsAffected>().HasNoKey();
              modelBuilder.Entity<MachineInformation>(entity =>
            {
                entity.HasKey(e => new { e.machine_id });
            });
                  modelBuilder.Entity<M_Users>(entity =>
            {
                entity.HasKey(e => new { e.account });
            });
        }

    }
}
