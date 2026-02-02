using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using AppointmentSystem.Domain.Entities;
using AppointmentSystem.Domain.Exceptions;
using AppointmentSystem.Domain.Repositories;
using MediatR;

namespace AppointmentSystem.Application.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler
    : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AppointmentDto>> Handle(
        GetAppointmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id, cancellationToken);

        if (appointment == null)
        {
            return Result.Failure<AppointmentDto>($"Appointment with ID {request.Id} not found");
        }

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