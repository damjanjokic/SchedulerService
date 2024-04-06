using MongoDB.Bson;

namespace SchedulerService.Dto;

public class ConfirmAppointmentDto
{
    public ObjectId AppointmentId { get; set; }
    public Guid UserId { get; set; }
}