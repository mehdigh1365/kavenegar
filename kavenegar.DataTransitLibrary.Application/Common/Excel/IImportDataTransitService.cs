using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Application.Common.Excel
{
    public interface IImportDataTransitService
    {
        Task ImportDataTransiFromExcel(IFormFile fileName, CancellationToken cancellationToken);
    }
}
