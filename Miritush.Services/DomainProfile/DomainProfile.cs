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

        }
    }
}