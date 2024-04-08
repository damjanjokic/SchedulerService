using MongoDB.Bson;
using SchedulerService.Dto;

namespace SchedulerService.Contracts;

public interface IAppointmentService
{
    Task<AppointmentDto> GetById(ObjectId id);
    Task<List<AppointmentDto>> GetAllByUserId(Guid userId);
    Task<List<AppointmentDto>> GetPendingAppointmentsForUser(Guid userId);
    Task ScheduleAppointment(ScheduleAppointmentDto scheduleAppointmentDto);
    Task ConfirmAppointment(ConfirmAppointmentDto confirmAppointmentDto);
    Task CreateApointment(CreateAppointmentDto createAppointmentDto);

}