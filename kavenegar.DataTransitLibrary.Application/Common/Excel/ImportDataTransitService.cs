using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire;
using Hangfire.Logging;
using kavenegar.DataTransitLibrary.Application.Core.DataTransit.Dto;
using kavenegar.DataTransitLibrary.Application.Core.DataTransits.Dto;
using Kavenegar.DataTransitLibrary.Common.Extensions;
using Kavenegar.DataTransitLibrary.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Application.Common.Excel
{
    public class ImportDataTransitService : IImportDataTransitService
    {
        private readonly IDistributedCache _cache;
        public ImportDataTransitService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task ImportDataTransiFromExcel(IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream, cancellationToken);
                using var excelWorkbook = new XLWorkbook(stream);

                //skip first row because its Definition Schema
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1).ToList();

                int page = 0;
                //بسته به منابع سرور این اعداد می تواند متغیر باشد
                int RowsCount = 4;
                int taskCount = 8;
                var allTask = new List<Task>();
                var DataTransitList = new List<DataTransit>();
                while (page < taskCount)
                {
                    var excelRows = nonEmptyDataRows
                        .Skip(page * RowsCount)
                        .Take(RowsCount)
                        .ToList();

                    if (excelRows?.Count > 0)
                    {
                        var t = Task.Factory.StartNew((object obj) =>
                        {
                            var data = obj as List<IXLRangeRow>;
                            if (data == null)
                                return;
                        }, excelRows);
                        allTask.Add(t);

                    }
                    else
                    {
                        break;
                    }

                    page++;
                }

                await Task.WhenAll(allTask);

                foreach (var task in allTask)
                {

                    var rows = task.AsyncState as List<IXLRangeRow>;
                    if (rows != null)
                    {
                        var DataTransits = new List<ImportDataTransitDto>();
                        foreach (var row in rows)
                        {
                            DataTransits.Add(new ImportDataTransitDto
                            {
                                Title = row.Cell(1).GetValue<string>()
                            });
                        }
                        //lock (task)
                        //{

                        var DataTransitsFromRedis = await _cache.GetRecordAsync<List<ImportDataTransitDto>>("getAllDataTransit", cancellationToken).ConfigureAwait(false);
                        if (DataTransitsFromRedis?.Count > 0)
                        {
                            _cache.RemoveRecordAsync("getAllDataTransit", cancellationToken).GetAwaiter().GetResult();
                            foreach (var item in DataTransits)
                            {
                                if (!DataTransitsFromRedis.Any(d => d.Title == item.Title))
                                {
                                    DataTransitsFromRedis.Add(new ImportDataTransitDto
                                    {
                                        Title = item.Title
                                    });
                                }

                            }
                            await _cache.SetRecordAsync("getAllDataTransit", DataTransitsFromRedis, cancellationToken).ConfigureAwait(false);
                        }
                        else
                        {
                            var PrepareRedisData = new List<ImportDataTransitDto>();
                            var result = DataTransits.Select(s => s.Title).Distinct().ToList();
                            foreach (var item in result)
                            {
                                PrepareRedisData.Add(new ImportDataTransitDto
                                {
                                    Title = item
                                });
                            }
                            await _cache.SetRecordAsync("getAllDataTransit", PrepareRedisData, cancellationToken).ConfigureAwait(false);

                        }
                        //}

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void WriteToRedis()
        {

        }
    }
}
