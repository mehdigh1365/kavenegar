using Kavenegar.DataTransitLibrary.Common.Interfaces;
using Kavenegar.DataTransitLibrary.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataTransitLibraryContext _context;
        public UnitOfWork(DataTransitLibraryContext context)
        {
            _context = context;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
    }
}
