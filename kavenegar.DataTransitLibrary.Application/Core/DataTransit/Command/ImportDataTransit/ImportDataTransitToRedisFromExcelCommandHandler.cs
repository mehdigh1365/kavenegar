using Hangfire;
using kavenegar.DataTransitLibrary.Application.Common.Excel;
using Kavenegar.DataTransitLibrary.Common.Extensions;
using Kavenegar.DataTransitLibrary.Common.Interfaces;
using Kavenegar.DataTransitLibrary.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Application.Core.DataTransit.Command.ImportDataTransit
{
    public class ImportDataTransitToRedisFromExcelCommandHandler : IRequestHandler<ImportDataTransitToRedisFromExcelCommand, Result>
    {
        private readonly IImportDataTransitService _importDataTransitService;
        public ImportDataTransitToRedisFromExcelCommandHandler(IImportDataTransitService importDataTransitService)
        {
            _importDataTransitService = importDataTransitService;
        }

        public Task<Result> Handle(ImportDataTransitToRedisFromExcelCommand request, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue(() => _importDataTransitService.ImportDataTransiFromExcel(request.File, cancellationToken).ConfigureAwait(false));
            return Task.FromResult(Result.SuccessFul());

        }
    }
}
