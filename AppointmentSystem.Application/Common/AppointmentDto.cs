namespace AppointmentSystem.Application.Appointments.Common;

public record AppointmentDto
{
    public int Id { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public DateTime AppointmentDate { get; init; }
    public string? Description { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}