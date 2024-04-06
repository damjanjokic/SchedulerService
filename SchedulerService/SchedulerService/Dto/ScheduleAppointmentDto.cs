using MongoDB.Bson;

namespace SchedulerService.Dto;

public class ScheduleAppointmentDto
{
    public ObjectId Id { get; set; }
    public string Description { get; set; }
    public List<Guid> Attendees { get; set; }
}