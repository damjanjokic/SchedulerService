using AutoMapper;
using SchedulerService.Dto;
using SchedulerService.Entities;

namespace SchedulerService.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppointmentDto, Appointment>();
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<ScheduleAppointmentDto, Appointment>();
        CreateMap<CreateAppointmentDto, Appointment>();
    }
}