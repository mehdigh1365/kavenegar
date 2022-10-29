using Kavenegar.DataTransitLibrary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Interfaces
{
    public interface IDataTransitLibraryContext
    {
        DbSet<DataTransit> DataTransits { get; set; }
        ChangeTracker ChangeTracker { get; }

        Task SaveAsync(CancellationToken cancellationToken);

        Task CloseConnection();

        void Save();
    }
}
