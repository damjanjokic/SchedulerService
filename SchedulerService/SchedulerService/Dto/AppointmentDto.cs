using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchedulerService.Dto;

public class AppointmentDto
{
    public ObjectId Id { get; set; }
    public Guid OrganisationId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public Guid OrganiserId { get; set; }
    public bool OrganiserConfirmationPending { get; set; }
    public bool Set { get; set; }
    public List<Guid> Attendees { get; set; }
}