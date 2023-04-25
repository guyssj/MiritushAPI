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

            CreateMap<DAL.Model.ProductCategory, DTO.ProductCategory>();


            CreateMap<DAL.Model.Transaction, DTO.Transaction>()
                .ForMember(x => x.CustomerName, config => config.MapFrom(x =>
                    x.Customer != null ? $"{x.Customer.FirstName} {x.Customer.LastName}" : null))
                .ForMember(x => x.Items, config => config.MapFrom(x => x.TransactionItems));

            CreateMap<DAL.Model.TransactionItem, DTO.TransactionItem>()
                .ForMember(x => x.ProductName, config => config.MapFrom(x =>
                    x.Product != null ? x.Product.Name : null))
                .ForMember(x => x.ServiceTypeName, config => config.MapFrom(x =>
                    x.ServiceType != null ? x.ServiceType.ServiceTypeName : null));

            CreateMap<DAL.Model.Product, DTO.Product>()
                .ForMember(x => x.Category, conifg => conifg.MapFrom(x => x.Category));

            CreateMap<DAL.Model.Servicetype, DTO.ServiceType>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.ServiceTypeId))
                .ForMember(x => x.Name, config => config.MapFrom(x => x.ServiceTypeName));

            CreateMap<DAL.Model.User, DTO.User>()
                .ForMember(x => x.Name, config => config.MapFrom(x => x.UserName));

            CreateMap<DAL.Model.Book, DTO.Book>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.BookId))
                .ForMember(x => x.Arrival, config => config.MapFrom(x => x.ArrivalStatus))
                .ForMember(x => x.Duration, config => config.MapFrom(x => x.Durtion));

            CreateMap<DAL.Model.User, DTO.BookIdentity>()
                .ForMember(x => x.Name, config => config.MapFrom(x => x.UserName))
                .ForMember(x => x.UserId, config => config.MapFrom(x => x.Id));
            //.ForMember(x => x.RoleName, config => config.MapFrom(x => x.Id > 0 ? "Admin" : "User"));

            CreateMap<DAL.Model.Customer, DTO.BookIdentity>()
                .ForMember(x => x.Name, config => config.MapFrom(x => x.PhoneNumber))
                .ForMember(x => x.UserId, config => config.MapFrom(x => x.CustomerId))
                .ForMember(x => x.RoleName, config => config.MapFrom(x => !string.IsNullOrWhiteSpace(x.PhoneNumber) ? "User" : "Admin"));

            CreateMap<DAL.Model.Lockhour, DTO.LockHour>();
            CreateMap<DAL.Model.Workhour, DTO.WorkHour>();
            CreateMap<DAL.Model.CustomerTimeline, DTO.CustomerTimeline>();

            CreateMap<DAL.Model.Attachment, DTO.Attachment>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.AttachmentId))
                .ForMember(x => x.Name, config => config.MapFrom(x => x.AttachmentName));



            CreateMap<DAL.Model.Closeday, DTO.CloseDay>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.CloseDaysId));


            CreateMap<DAL.Model.Book, DTO.CalendarEvent<DTO.Book>>()
                .ForMember(x => x.Meta, config => config.MapFrom(x => new DTO.Book()
                {
                    Id = x.BookId,
                    CustomerId = x.CustomerId,
                    ServiceId = x.ServiceId,
                    ServiceTypeId = x.ServiceTypeId,
                    Duration = x.Durtion,
                    StartDate = x.StartDate,
                    StartAt = x.StartAt,
                    Notes = x.Notes
                }))
                .ForMember(x => x.StartTime, config => config.MapFrom(
                     x => x.StartDate.AddMinutes(x.StartAt)))
                .ForMember(x => x.EndTime, config => config.MapFrom(
                     x => x.StartDate.AddMinutes(x.StartAt + x.Durtion)));

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