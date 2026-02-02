using AppointmentSystem.Domain.Entities;
using AppointmentSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentSystem.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.PatientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.AppointmentDate)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), v));

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.HasIndex(a => a.PatientName);
        builder.HasIndex(a => a.AppointmentDate);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => new { a.Status, a.AppointmentDate });
        builder.HasIndex(a => new { a.PatientName, a.AppointmentDate });

        builder.Ignore(a => a.DomainEvents);
    }
}