using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SchedulerService.Contracts;
using SchedulerService.Data;
using SchedulerService.Dto;
using SchedulerService.Entities;

namespace SchedulerService.Controllers;

[Route(("api/[controller]"))]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    
    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(ObjectId id)
    {
        var appointment = await _appointmentService.GetById(id);
        
        if (appointment == null)
            return NotFound();
        
        return Ok(appointment);
    }
    
    [HttpGet("getAppointmentsForUser/{userId}")]
    public async Task<IActionResult> GetAllAppointmentsByUserId(Guid userId)
    {
        var appointments = await _appointmentService.GetAllByUserId(userId);
        
        if (!appointments.Any())
            return NotFound();
        
        return Ok(appointments);
    }
    
    [HttpGet("getPendingAppointmentsForUser/{userId}")]
    public async Task<IActionResult> GetPendingAppointmentsForUser(Guid userId)
    {

        var appointments = await _appointmentService.GetPendingAppointmentsForUser(userId);
        
        if (!appointments.Any())
            return NotFound();
        
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAppointment(CreateAppointmentDto createAppointmentDto)
    {
        await _appointmentService.CreateApointment(createAppointmentDto);
        return Ok();
    }

    [HttpPut("scheduleAppointment")]
    public async Task<ActionResult> ScheduleAppointment(ScheduleAppointmentDto scheduleAppointmentDto)
    {

        await _appointmentService.ScheduleAppointment(scheduleAppointmentDto);
        return Ok();
    }

    [HttpPut("confirmAppointment")]
    public async Task<IActionResult> ConfirmAppointment(ConfirmAppointmentDto confirmAppointmentDto)
    {

        await _appointmentService.ConfirmAppointment(confirmAppointmentDto);
        return Ok();
    }

}