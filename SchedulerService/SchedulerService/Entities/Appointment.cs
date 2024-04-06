using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchedulerService.Entities;

public class Appointment
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("organisation_id"), BsonRepresentation(BsonType.Binary)]
    public Guid OrganisationId { get; set; }
    [BsonElement("date"), BsonRepresentation(BsonType.DateTime)]
    public DateTime Date { get; set; }
    [BsonElement("start_time"), BsonRepresentation(BsonType.DateTime)]
    public DateTime StartTime { get; set; }
    [BsonElement("end_time"), BsonRepresentation(BsonType.DateTime)]
    public DateTime EndTime { get; set; }
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string Description { get; set; }
    [BsonElement("organiser_id"), BsonRepresentation(BsonType.Binary)]
    public Guid OrganiserId { get; set; }
    [BsonElement("organiser_approval_pending"), BsonRepresentation(BsonType.Boolean)]
    public bool OrganiserApprovalPending { get; set; }
    [BsonElement("set"), BsonRepresentation(BsonType.Boolean)]
    public bool Set { get; set; }
    [BsonElement("attendees")]
    public List<Guid> Attendees { get; set; }
}