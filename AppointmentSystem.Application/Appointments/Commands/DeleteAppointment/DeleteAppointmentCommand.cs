using AppointmentSystem.Application.Common.Models;
using MediatR;

namespace AppointmentSystem.Application.Appointments.Commands.DeleteAppointment;

public record DeleteAppointmentCommand(int Id) : IRequest<Result>;