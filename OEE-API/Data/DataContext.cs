using Microsoft.EntityFrameworkCore;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<M_Users> M_Users {get;set;}
        public DbSet<M_ActionTimeForOEE> M_ActionTimeForOEE {get;set;}
        public DbSet<M_DowntimeRecord> M_DowntimeRecord {get;set;}
        public DbSet<M_DowntimeReason> M_DowntimeReason {get;set;}
        public DbSet<M_Factory> M_Factory {get;set;}
        public DbSet<M_MachineInformation> M_MachineInformation {get;set;}
        public DbSet<M_MachineStatus> M_MachineStatus {get;set;}
        public DbSet<M_MachineType> M_MachineType {get;set;}
        public DbSet<M_MaintenanceTime> M_MaintenanceTime {get;set;}
        public DbSet<M_OEE_ID> M_OEE_ID {get;set;}
        public DbSet<M_OEE_MM> M_OEE_MM {get;set;}
        public DbSet<M_OEE_VN> M_OEE_VN {get;set;}
        public DbSet<M_Roles> M_Roles {get;set;}
        public DbSet<M_RoleUser> M_RoleUser {get;set;}
        public DbSet<M_RowIndex> M_RowIndex {get;set;}
        public DbSet<M_Shift> M_Shift {get;set;}
        public DbSet<M_ShiftTimeConfig> M_ShiftTimeConfig {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_ActionTimeForOEE>().HasKey(x => new {x.id, x.date});
            modelBuilder.Entity<M_OEE_ID>().HasKey(x => new {
                x.factory_id, x.building_id, 
                x.machine_id, x.date,
                x.time_group_for_oee
            });
            modelBuilder.Entity<M_OEE_MM>().HasKey(x => new {
                x.factory_id, x.building_id,
                x.machine_id, x.date,
                x.time_group_for_oee
            });
            modelBuilder.Entity<M_OEE_VN>().HasKey(x => new {
                x.factory_id, x.building_id,
                x.machine_id, x.date,
                x.time_group_for_oee
            });
            modelBuilder.Entity<M_ShiftTimeConfig>().HasKey(x => new {
                x.factory_id, x.building_id,
                x.shift_id, x.start_time,
                x.end_time
            });
        }
    }
}