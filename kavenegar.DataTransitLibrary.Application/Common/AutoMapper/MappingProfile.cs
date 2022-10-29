using AutoMapper;
using kavenegar.DataTransitLibrary.Application.Core.DataTransits.Dto;
using Kavenegar.DataTransitLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Application.Common.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DataTransit, DataTransitDto>();
        }
    }
}
