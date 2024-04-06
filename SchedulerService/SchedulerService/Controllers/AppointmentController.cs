using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SchedulerService.Data;
using SchedulerService.Dto;
using SchedulerService.Entities;

namespace SchedulerService.Controllers;

[Route(("api/[controller]"))]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IMongoCollection<Appointment> _appointments;
    private readonly IMapper _mapper;
    public AppointmentController(MongoDbService mongoDbService, IMapper mapper)
    {
        _mapper = mapper;
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
        var appointmetDto = _mapper.Map<AppointmentDto>(appointment);
        return appointmetDto is not null ? Ok(appointmetDto) : NotFound();
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointmentsByUserId(Guid userId)
    {
        var filter = Builders<Appointment>.Filter.Eq(x => x.OrganiserId, userId);
        var appointments = await _appointments.Find(filter).ToListAsync();
        var appointmetsDto = _mapper.Map<List<AppointmentDto>>(appointments);
        return appointmetsDto is not null ? Ok(appointmetsDto) : NotFound();
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsWaitingForConfirmation(Guid userId)
    {
        var filter = Builders<Appointment>.Filter.Eq(x => x.OrganiserId, userId) &
                     Builders<Appointment>.Filter.Eq(x => x.OrganiserConfirmationPending, true) &
                     Builders<Appointment>.Filter.Eq(x => x.Set, false);
        
        var appointments = await _appointments.Find(filter).ToListAsync();
        var appointmetsDto = _mapper.Map<List<AppointmentDto>>(appointments);
        
        return appointmetsDto is not null ? Ok(appointmetsDto) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateAppointmentDto createAppointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(createAppointmentDto);
        appointment.Id = ObjectId.GenerateNewId();
        if (appointment.Attendees.Contains(appointment.OrganiserId))
        {
            appointment.OrganiserConfirmationPending = false;
            appointment.Set = true;
        }
        await _appointments.InsertOneAsync(appointment);
        return Ok(appointment.Id);
    }

    [HttpPut]
    public async Task<ActionResult> ScheduleAppointment(ScheduleAppointmentDto scheduleAppointmentDto)
    {
        var filter = Builders<Appointment>.Filter.Eq(x => x.Id, scheduleAppointmentDto.Id) &
                     Builders<Appointment>.Filter.Eq(x => x.OrganiserConfirmationPending, false) &
                     Builders<Appointment>.Filter.Eq(x => x.Set, false);

        var update = Builders<Appointment>.Update
            .Set(x => x.Description, scheduleAppointmentDto.Description)
            .Set(x => x.Attendees, scheduleAppointmentDto.Attendees)
            .Set(x => x.OrganiserConfirmationPending, true);
        await _appointments.UpdateOneAsync(filter, update);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> ConfirmAppointment(ConfirmAppointmentDto confirmAppointmentDto)
    {
        var filter = Builders<Appointment>.Filter.Eq(x => x.Id, confirmAppointmentDto.AppointmentId) &
                     Builders<Appointment>.Filter.Eq(x => x.OrganiserId, confirmAppointmentDto.UserId) &
                     Builders<Appointment>.Filter.Eq(x => x.OrganiserConfirmationPending, true);
                     Builders<Appointment>.Filter.Eq(x => x.Set, false); 
        
        var update = Builders<Appointment>.Update
            .Set(x => x.OrganiserConfirmationPending, false)
            .Set(x => x.Set, true);
        await _appointments.UpdateOneAsync(filter, update);

        return Ok();
    }

}