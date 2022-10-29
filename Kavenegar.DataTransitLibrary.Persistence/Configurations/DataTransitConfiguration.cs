using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kavenegar.DataTransitLibrary.Domain.Models;

namespace Kavenegar.DataTransitLibrary.Persistence.Configurations
{
    public class DataTransitConfiguration : IEntityTypeConfiguration<DataTransit>
    {
        public void Configure(EntityTypeBuilder<DataTransit> builder)
        {
            builder.HasKey(e => e.DataTransitId);

            builder.Property(e => e.Title).IsRequired();

            builder.Property(e => e.Title).HasMaxLength(50);
        }
    }
}
