using kavenegar.DataTransitLibrary.Application.Core.DataTransits.Dto;
using Kavenegar.DataTransitLibrary.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Application.Core.DataTransit.Command.ImportDataTransit
{
    public class ImportDataTransitToRedisFromExcelCommand : IRequest<Result>
    {
        public IFormFile File { get; set; }
    }
}
