using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using MediatR;

namespace AppointmentSystem.Application.Appointments.Queries.GetAppointmentById;

public record GetAppointmentByIdQuery(int Id) : IRequest<Result<AppointmentDto>>;