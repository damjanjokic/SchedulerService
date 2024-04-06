namespace SchedulerService.Dto;

public class CreateAppointmentDto
{

    public Guid OrganisationId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public Guid OrganiserId { get; set; }
    public List<Guid> Attendees { get; set; }
}