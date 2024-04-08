using System.Net;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using SchedulerService.Contracts;
using SchedulerService.Data;
using SchedulerService.Dto;
using SchedulerService.Entities;

namespace SchedulerService.Services;

public class AppointmentService : IAppointmentService
{
    //private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Appointment> _appointments;

    public AppointmentService(MongoDbService mongoDbService, IMapper mapper)
    {
        _mapper = mapper;
        _appointments = mongoDbService.Database?.GetCollection<Appointment>("appointment");
        //_logger = logger;
    }
    
    public async Task<AppointmentDto> GetById(ObjectId id)
    {
        try
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.Id, id);
            var appointment = await _appointments.Find(filter).FirstOrDefaultAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }
        catch (Exception e)
        {
            //_logger.LogInformation("Method: AppointmentService.GetById, Error: " + e.Message);
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<List<AppointmentDto>> GetAllByUserId(Guid userId)
    {
        try
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.OrganiserId, userId);
            var appointments = await _appointments.Find(filter).ToListAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
        catch (Exception e)
        {
            //_logger.LogInformation("Method: AppointmentService.GetAllByUserId, Error: " + e.Message);
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<List<AppointmentDto>> GetPendingAppointmentsForUser(Guid userId)
    {
        try
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.OrganiserId, userId) &
                         Builders<Appointment>.Filter.Eq(x => x.OrganiserConfirmationPending, true) &
                         Builders<Appointment>.Filter.Eq(x => x.Set, false);

            var appointments = await _appointments.Find(filter).ToListAsync();
            
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
        catch (Exception e)
        {
            //_logger.LogInformation("Method: AppointmentService.GetById, GetPendingAppointmentsForUser: " + e.Message);
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ScheduleAppointment(ScheduleAppointmentDto scheduleAppointmentDto)
    {
        try
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.Id, scheduleAppointmentDto.Id) &
                         Builders<Appointment>.Filter.Eq(x => x.OrganiserConfirmationPending, false) &
                         Builders<Appointment>.Filter.Eq(x => x.Set, false);

            var update = Builders<Appointment>.Update
                .Set(x => x.Description, scheduleAppointmentDto.Description)
                .Set(x => x.Attendees, scheduleAppointmentDto.Attendees)
                .Set(x => x.OrganiserConfirmationPending, true);
            
            await _appointments.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            //_logger.LogInformation("Method: AppointmentService.ScheduleAppointment, Error: " + e.Message);
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ConfirmAppointment(ConfirmAppointmentDto confirmAppointmentDto)
    {
        try
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.Id, confirmAppointmentDto.AppointmentId) &
                         Builders<Appointment>.Filter.Eq(x => x.OrganiserId, confirmAppointmentDto.UserId) &
                         Builders<Appointment>.Filter.Eq(x => x.OrganiserConfirmationPending, true);
            Builders<Appointment>.Filter.Eq(x => x.Set, false); 
        
            var update = Builders<Appointment>.Update
                .Set(x => x.OrganiserConfirmationPending, false)
                .Set(x => x.Set, true);
            
            await _appointments.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            //_logger.LogInformation("Method: AppointmentService.ConfirmAppointment, Error: " + e.Message);
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateApointment(CreateAppointmentDto createAppointmentDto)
    {
        try
        {
            var appointment = _mapper.Map<Appointment>(createAppointmentDto);
            appointment.Id = ObjectId.GenerateNewId();
            if (appointment.Attendees.Contains(appointment.OrganiserId))
            {
                appointment.OrganiserConfirmationPending = false;
                appointment.Set = true;
            }
            await _appointments.InsertOneAsync(appointment);
        }
        catch (Exception e)
        {
            //_logger.LogInformation("Method: AppointmentService.GetById, Error: " + e.Message);
            Console.WriteLine(e);
            throw;
        }
    }
}