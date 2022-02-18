using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Miritush.Services.DomainProfile
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<DAL.Model.Customer, DTO.Customer>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.CustomerId));
            CreateMap<DAL.Model.Service, DTO.Service>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.ServiceId));

            CreateMap<DAL.Model.Servicetype, DTO.ServiceType>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.ServiceTypeId))
                .ForMember(x => x.Name, config => config.MapFrom(x => x.ServiceTypeName));


            CreateMap<DAL.Model.User, DTO.User>()
                .ForMember(x=>x.Name,config =>config.MapFrom(x=>x.UserName));


            CreateMap<DAL.Model.Lockhour, DTO.LockHour>();
            CreateMap<DAL.Model.Workhour, DTO.WorkHour>();


            CreateMap<DAL.Model.Closeday, DTO.CloseDay>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.CloseDaysId));

            CreateMap<DAL.Model.Lockhour, DTO.CalendarEvent<DTO.LockHour>>()
                .ForMember(x => x.Meta, config => config.MapFrom(x => new DTO.LockHour()
                {
                    IdLockHours = x.IdLockHours,
                    StartDate = x.StartDate,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Notes = x.Notes
                }))
                .ForMember(x => x.StartTime, config => config.MapFrom(
                     x => x.StartDate.AddMinutes(x.StartAt)))
                .ForMember(x => x.EndTime, config => config.MapFrom(
                     x => x.StartDate.AddMinutes(x.EndAt)))
                .ForMember(x => x.Title, config => config.MapFrom(x => "זמן נעול"));

        }
    }
}