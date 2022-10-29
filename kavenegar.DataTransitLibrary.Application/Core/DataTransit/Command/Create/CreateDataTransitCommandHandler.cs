using ClosedXML.Excel;
using Hangfire;
using kavenegar.DataTransitLibrary.Application.Core.DataTransit.Dto;
using kavenegar.DataTransitLibrary.Application.Core.DataTransits.Dto;
using Kavenegar.DataTransitLibrary.Common.Extensions;
using Kavenegar.DataTransitLibrary.Common.Interfaces;
using Kavenegar.DataTransitLibrary.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Application.Core.DataTransit.Command.Create
{
    public class CreateDataTransitCommandHandler : IRequestHandler<CreateDataTransitCommand, Result>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDistributedCache _cache;
        public CreateDataTransitCommandHandler(
            IDistributedCache cache,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _cache = cache;
        }
        public Task<Result> Handle(CreateDataTransitCommand request, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue(() => ReadDataFromRedisAndWriteToDb(cancellationToken).ConfigureAwait(false));
            return Task.FromResult(Result.SuccessFul());
        }
        private async Task ReadDataFromRedisAndWriteToDb(CancellationToken cancellationToken)
        {
            try
            {
                var DataTransitsFromRedis = await _cache.GetRecordAsync<List<ImportDataTransitDto>>("getAllDataTransit", cancellationToken).ConfigureAwait(false);
                if (DataTransitsFromRedis?.Count > 0)
                {

                    int page = 0;
                    //بسته به منابع سرور این اعداد می تواند متغیر باشد
                    int RowsCount = 4;
                    int taskCount = 8;
                    var allTask = new List<Task>();
                    while (page < taskCount)
                    {
                        var redisDatas = DataTransitsFromRedis
                            .Skip(page * RowsCount)
                            .Take(RowsCount)
                            .ToList();

                        if (redisDatas?.Count > 0)
                        {
                            var t = Task.Factory.StartNew((object obj) =>
                            {
                                var data = obj as List<ImportDataTransitDto>;
                                if (data == null)
                                    return;
                            }, redisDatas);
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
                        var dataTransitFromRedis = task.AsyncState as List<ImportDataTransitDto>;
                        if (dataTransitFromRedis != null)
                        {
                            foreach (var dataTransit in dataTransitFromRedis)
                            {
                                if (!await ExistsDataTransit(dataTransit.Title).ConfigureAwait(false))
                                {
                                    using var scope = _serviceScopeFactory.CreateScope();
                                    var context = scope.ServiceProvider.GetService<IDataTransitLibraryContext>();
                                    
                                    await context.DataTransits.AddAsync(new Kavenegar.DataTransitLibrary.Domain.Models.DataTransit
                                    {
                                        Title = dataTransit.Title
                                    }).ConfigureAwait(false);

                                    await context.SaveAsync(cancellationToken).ConfigureAwait(false);
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<bool> ExistsDataTransit(string title)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IDataTransitLibraryContext>();

            var dataTransitTitle = await context.DataTransits.AsNoTracking()
                .Where(d => d.Title == title)
                .Select(dt => dt.Title).SingleOrDefaultAsync();

            return !string.IsNullOrEmpty(dataTransitTitle)  ? true : false;
        }
    }
}
