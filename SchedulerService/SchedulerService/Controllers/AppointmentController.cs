using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SchedulerService.Data;
using SchedulerService.Entities;

namespace SchedulerService.Controllers;

[Route(("api/[controller]"))]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IMongoCollection<Appointment> _appointments;
    public AppointmentController(MongoDbService mongoDbService)
    {
        _appointments = mongoDbService.Database?.GetCollection<Appointment>("appointment");
    }

    [HttpGet]
    public async Task<IEnumerable<Appointment>> Get()
    {
        return await _appointments.Find(FilterDefinition<Appointment>.Empty).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetById(ObjectId id)
    {
        var filter = Builders<Appointment>.Filter.Eq(x => x.Id, id);
        var appointment = await _appointments.Find(filter).FirstOrDefaultAsync();
        return appointment is not null ? Ok(appointment) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Post(Appointment appointment)
    {
        appointment.Id = ObjectId.GenerateNewId();
        await _appointments.InsertOneAsync(appointment);
        return Ok(appointment.Id);
    }
    
}