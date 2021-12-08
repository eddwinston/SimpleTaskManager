using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Domain;

namespace SimpleTaskManager.Data.Sql
{
    public class SimpleTaskManagerContext : DbContext
    {
        public SimpleTaskManagerContext(DbContextOptions<SimpleTaskManagerContext> options) : base(options)
        {

        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BoardUser> BoardUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Board>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<TaskItem>().HasIndex(x => x.Title).IsUnique();
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            builder
                .Entity<Board>()
                .HasMany(board => board.Tasks)
                .WithOne(task => task.Board)
                .HasForeignKey(task => task.BoardId);

            builder
                .Entity<TaskItem>()
                .HasOne(task => task.User)
                .WithMany(user => user.Tasks)
                .HasForeignKey(task => task.UserId);

            builder
                .Entity<User>()
                .HasMany(user => user.Tasks)
                .WithOne(task => task.User);

            builder
                .Entity<BoardUser>()
                .ToTable("BoardUsers")
                .HasKey(bu => new { bu.BoardId, bu.UserId });
            builder
                .Entity<BoardUser>()
                .HasOne(bu => bu.Board)
                .WithMany(bu => bu.BoardUsers)
                .HasForeignKey(bu => bu.BoardId);
            builder
                .Entity<BoardUser>()
                .HasOne(bu => bu.User)
                .WithMany(bu => bu.BoardUsers)
                .HasForeignKey(bu => bu.UserId);
        }
    }
}
