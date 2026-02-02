using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using MediatR;

namespace AppointmentSystem.Application.Appointments.Commands.UpdateAppointment;

public record UpdateAppointmentCommand : IRequest<Result<AppointmentDto>>
{
    public int Id { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public DateTime AppointmentDate { get; init; }
    public string? Description { get; init; }
}