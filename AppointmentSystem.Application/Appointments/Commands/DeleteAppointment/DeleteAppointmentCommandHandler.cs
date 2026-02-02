using AppointmentSystem.Application.Common.Models;
using AppointmentSystem.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentSystem.Application.Appointments.Commands.DeleteAppointment;

public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAppointmentCommandHandler> _logger;

    public DeleteAppointmentCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteAppointmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id, cancellationToken);

        if (appointment == null)
        {
            return Result.Failure($"Appointment with ID {request.Id} not found");
        }

        _unitOfWork.Appointments.Delete(appointment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted appointment {AppointmentId}", request.Id);

        return Result.Success();
    }
}