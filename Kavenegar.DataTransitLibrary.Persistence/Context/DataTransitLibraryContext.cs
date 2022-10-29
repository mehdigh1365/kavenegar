using Kavenegar.DataTransitLibrary.Common.Interfaces;
using Kavenegar.DataTransitLibrary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Persistence.Context
{
    public class DataTransitLibraryContext : DbContext, IDataTransitLibraryContext
    {
        public DataTransitLibraryContext()
        {

        }
        public DataTransitLibraryContext(DbContextOptions<DataTransitLibraryContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=.;Initial Catalog =DataTransitLibraryContext;MultipleActiveResultSets=true;User ID=sa;Password=123");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataTransitLibraryContext).Assembly);

        public DbSet<DataTransit> DataTransits { get; set; }

        public Task CloseConnection() => base.Database.CloseConnectionAsync();

        public void Save() => base.SaveChanges();

        public Task SaveAsync(CancellationToken cancellationToken) => base.SaveChangesAsync(cancellationToken);
    }
}
