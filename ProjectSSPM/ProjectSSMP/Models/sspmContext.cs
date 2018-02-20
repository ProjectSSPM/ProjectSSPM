using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectSSMP.Models
{
    public partial class sspmContext : DbContext
    {
        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<Bulletin> Bulletin { get; set; }
        public virtual DbSet<BulletinChat> BulletinChat { get; set; }
        public virtual DbSet<DeviceNumber> DeviceNumber { get; set; }
        public virtual DbSet<Function> Function { get; set; }
        public virtual DbSet<FunctionLog> FunctionLog { get; set; }
        public virtual DbSet<MenuAuthentication> MenuAuthentication { get; set; }
        public virtual DbSet<MenuGroup> MenuGroup { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<RunningNumber> RunningNumber { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TeamTask> TeamTask { get; set; }
        public virtual DbSet<TimeSheet> TimeSheet { get; set; }
        public virtual DbSet<UserAssignGroup> UserAssignGroup { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserImage> UserImage { get; set; }
        public virtual DbSet<UserSspm> UserSspm { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=den1.mssql5.gear.host;Initial Catalog=sspm;Integrated Security=False;User ID=sspm;Password=Gi90MMTY!H_i;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            
        }
        public sspmContext(DbContextOptions<sspmContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>(entity =>
            {
                entity.Property(e => e.ActionId)
                    .HasColumnName("ActionID")
                    .HasColumnType("char(1)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionName).HasMaxLength(30);
            });

            modelBuilder.Entity<Bulletin>(entity =>
            {
                entity.HasKey(e => e.Bnumber);

                entity.Property(e => e.Bnumber)
                    .HasColumnName("BNumber")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<BulletinChat>(entity =>
            {
                entity.HasKey(e => new { e.Bnumber, e.Bchat });

                entity.Property(e => e.Bnumber)
                    .HasColumnName("BNumber")
                    .HasMaxLength(10);

                entity.Property(e => e.Bchat)
                    .HasColumnName("BChat")
                    .HasMaxLength(10);

                entity.Property(e => e.Ctime)
                    .HasColumnName("CTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<DeviceNumber>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DeviceId).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(10);
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.HasKey(e => new { e.FunctionId, e.ProjectNumber, e.TaskId });

                entity.Property(e => e.FunctionId)
                    .HasColumnName("FunctionID")
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectNumber).HasMaxLength(10);

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasMaxLength(10);

                entity.Property(e => e.ActualEnd).HasColumnType("datetime");

                entity.Property(e => e.ActualStart).HasColumnType("datetime");

                entity.Property(e => e.FunctionEnd).HasColumnType("datetime");

                entity.Property(e => e.FunctionName).HasMaxLength(50);

                entity.Property(e => e.FunctionStart).HasColumnType("datetime");
            });

            modelBuilder.Entity<FunctionLog>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.ProjectNumber, e.FunctionLogId, e.FunctionNumber });

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectNumber).HasMaxLength(10);

                entity.Property(e => e.FunctionLogId)
                    .HasColumnName("FunctionLogID")
                    .HasColumnType("datetime");

                entity.Property(e => e.FunctionNumber).HasMaxLength(10);

                entity.Property(e => e.ActualEnd).HasColumnType("datetime");

                entity.Property(e => e.ActualStart).HasColumnType("datetime");

                entity.Property(e => e.Approve1).HasMaxLength(10);

                entity.Property(e => e.Approve2).HasMaxLength(10);

                entity.Property(e => e.FunctionEnd).HasColumnType("datetime");

                entity.Property(e => e.FunctionId)
                    .IsRequired()
                    .HasColumnName("FunctionID")
                    .HasMaxLength(10);

                entity.Property(e => e.FunctionStart).HasColumnType("datetime");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<MenuAuthentication>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.MenuId });

                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasMaxLength(2);

                entity.Property(e => e.MenuId)
                    .HasColumnName("MenuID")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<MenuGroup>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.Property(e => e.MenuId)
                    .HasColumnName("MenuID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.MenuIcon).HasMaxLength(50);

                entity.Property(e => e.MenuName).HasMaxLength(50);

                entity.Property(e => e.MenuUrl)
                    .HasColumnName("MenuURL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectNumber);

                entity.Property(e => e.ProjectNumber)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActualEnd).HasColumnType("datetime");

                entity.Property(e => e.ActualStart).HasColumnType("datetime");

                entity.Property(e => e.CustomerName).HasMaxLength(100);

                entity.Property(e => e.CustomerTel).HasMaxLength(50);

                entity.Property(e => e.Note).HasColumnType("text");

                entity.Property(e => e.ProjectCreateBy).HasMaxLength(30);

                entity.Property(e => e.ProjectCreateDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectEditBy).HasMaxLength(30);

                entity.Property(e => e.ProjectEditDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectEnd).HasColumnType("datetime");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectID")
                    .HasMaxLength(50);

                entity.Property(e => e.ProjectManager).HasMaxLength(10);

                entity.Property(e => e.ProjectName).HasMaxLength(100);

                entity.Property(e => e.ProjectStart).HasColumnType("datetime");

                entity.Property(e => e.ProjectStatus).HasColumnType("char(1)");
            });

            modelBuilder.Entity<RunningNumber>(entity =>
            {
                entity.HasKey(e => e.Type);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasColumnType("char(1)")
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.ProjectNumber });

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectNumber).HasMaxLength(10);

                entity.Property(e => e.ActualEnd).HasColumnType("datetime");

                entity.Property(e => e.ActualStart).HasColumnType("datetime");

                entity.Property(e => e.TaskEnd).HasColumnType("datetime");

                entity.Property(e => e.TaskName).HasMaxLength(100);

                entity.Property(e => e.TaskStart).HasColumnType("datetime");
            });

            modelBuilder.Entity<TeamTask>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.ProjectNumber, e.FunctionId });

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectNumber).HasMaxLength(10);

                entity.Property(e => e.FunctionId)
                    .HasColumnName("FunctionID")
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectResponsible).HasMaxLength(100);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<TimeSheet>(entity =>
            {
                entity.HasKey(e => new { e.FunctionId, e.TaskId, e.ProjectNumber, e.TimeSheetNumber });

                entity.Property(e => e.FunctionId)
                    .HasColumnName("FunctionID")
                    .HasMaxLength(10);

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectNumber).HasMaxLength(10);

                entity.Property(e => e.TimeSheetNumber).HasMaxLength(10);

                entity.Property(e => e.ActionId)
                    .HasColumnName("ActionID")
                    .HasColumnType("char(1)");

                entity.Property(e => e.Approve1).HasMaxLength(10);

                entity.Property(e => e.Approve2).HasMaxLength(10);

                entity.Property(e => e.TimeSheetEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeSheetId)
                    .HasColumnName("TimeSheetID")
                    .HasColumnType("datetime");

                entity.Property(e => e.TimeSheetStart).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<UserAssignGroup>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("GroupID")
                    .HasMaxLength(2);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasMaxLength(2)
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupName).HasMaxLength(30);
            });

            modelBuilder.Entity<UserImage>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Image).HasColumnType("binary(255)");
            });

            modelBuilder.Entity<UserSspm>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserSSPM");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.JobResponsible).HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(50);

                entity.Property(e => e.LineId)
                    .HasColumnName("LineID")
                    .HasMaxLength(30);

                entity.Property(e => e.Password).HasMaxLength(30);

                entity.Property(e => e.Status).HasColumnType("char(1)");

                entity.Property(e => e.UserCreateBy).HasMaxLength(30);

                entity.Property(e => e.UserCreateDate).HasColumnType("datetime");

                entity.Property(e => e.UserEditBy).HasMaxLength(30);

                entity.Property(e => e.UserEditDate).HasColumnType("datetime");

                entity.Property(e => e.UserTel).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(30);
            });
        }
    }
}
