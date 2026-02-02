using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using MediatR;

namespace AppointmentSystem.Application.Appointments.Queries.GetAppointments;

public record GetAppointmentsQuery : IRequest<Result<PagedResult<AppointmentDto>>>
{
    public string? SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}