using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace aplabs_nastya
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Car, CarDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Client, ClientDto>();
            CreateMap<Car, CarDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<ClientForCreationDto, Client>();
            CreateMap<CarForCreationDto, Car>();
            CreateMap<EmployeeForCreationDto, Employee>();
        }
    }
}
