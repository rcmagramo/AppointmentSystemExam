using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using MediatR;

namespace AppointmentSystem.Application.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand : IRequest<Result<AppointmentDto>>
{
    public string PatientName { get; init; } = string.Empty;
    public DateTime AppointmentDate { get; init; }
    public string? Description { get; init; }
}