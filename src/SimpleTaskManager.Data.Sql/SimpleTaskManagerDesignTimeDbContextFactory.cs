using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleTaskManager.Data.Sql
{
    public class SimpleTaskManagerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SimpleTaskManagerContext>
    {
        public SimpleTaskManagerContext CreateDbContext(string[] args)
        {
            const string connectionString = @"server=.\SQLExpress;database=SimpleTaskManagerDb;trusted_connection=true;";
            var builder = new DbContextOptionsBuilder<SimpleTaskManagerContext>();

            builder.UseSqlServer(connectionString,
                migration =>
                    migration.MigrationsAssembly("SimpleTaskManager.Data.Sql"));
            return new SimpleTaskManagerContext(builder.Options);
        }
    }
}
