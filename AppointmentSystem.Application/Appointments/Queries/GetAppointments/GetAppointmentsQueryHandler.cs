using AppointmentSystem.Application.Appointments.Common;
using AppointmentSystem.Application.Common.Models;
using AppointmentSystem.Domain.Entities;
using AppointmentSystem.Domain.Repositories;
using AppointmentSystem.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentSystem.Application.Appointments.Queries.GetAppointments;

public class GetAppointmentsQueryHandler
    : IRequestHandler<GetAppointmentsQuery, Result<PagedResult<AppointmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAppointmentsQueryHandler> _logger;

    public GetAppointmentsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetAppointmentsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PagedResult<AppointmentDto>>> Handle(
        GetAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting appointments page {PageNumber}", request.PageNumber);

        var specification = new AppointmentFilterSpecification(request.SearchTerm);

        var (appointments, totalCount) = await _unitOfWork.Appointments.GetPagedAsync(
            specification,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var dtos = appointments.Select(MapToDto).ToList();

        var pagedResult = new PagedResult<AppointmentDto>(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result.Success(pagedResult);
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

public class AppointmentFilterSpecification : BaseSpecification<Appointment>
{
    public AppointmentFilterSpecification(string? searchTerm)
        : base(a => string.IsNullOrWhiteSpace(searchTerm) ||
                    a.PatientName.Contains(searchTerm))
    {
        ApplyOrderBy(a => a.AppointmentDate);
    }
}