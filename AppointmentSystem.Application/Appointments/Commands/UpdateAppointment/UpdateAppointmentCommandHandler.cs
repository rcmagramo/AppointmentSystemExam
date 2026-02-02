using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using AppointmentSystem.Domain.Entities;
using AppointmentSystem.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentSystem.Application.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentCommandHandler
    : IRequestHandler<UpdateAppointmentCommand, Result<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateAppointmentCommandHandler> _logger;

    public UpdateAppointmentCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateAppointmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<AppointmentDto>> Handle(
        UpdateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id, cancellationToken);

        if (appointment == null)
        {
            return Result.Failure<AppointmentDto>($"Appointment with ID {request.Id} not found");
        }

        appointment.UpdatePatientName(request.PatientName);
        appointment.UpdateAppointmentDate(request.AppointmentDate);
        appointment.UpdateDescription(request.Description);

        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated appointment {AppointmentId}", appointment.Id);

        var dto = MapToDto(appointment);
        return Result.Success(dto);
    }

    private static AppointmentDto MapToDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientName = appointment.PatientName,
            AppointmentDate = appointment.AppointmentDate,
            Description = appointment.Description,
            Status = appointment.Status.ToString(),
            CreatedAt = appointment.CreatedAt,
            UpdatedAt = appointment.UpdatedAt
        };
    }
}