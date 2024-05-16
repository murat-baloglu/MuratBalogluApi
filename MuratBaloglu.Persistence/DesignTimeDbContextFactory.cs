using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MuratBalogluDbContext>
    {
        public MuratBalogluDbContext CreateDbContext(string[] args)
        {

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<MuratBalogluDbContext>();
            dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
            return new MuratBalogluDbContext(dbContextOptionsBuilder.Options);

        }
    }
}
