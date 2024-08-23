using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.SqlServer
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {

        }

        public DbSet<Address> Address { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
