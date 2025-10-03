using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<IncidentReport> IncidentReports { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<VolunteerTask> VolunteerTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure IncidentReport
        builder.Entity<IncidentReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(500);
            entity.Property(e => e.DateReported).IsRequired();
            entity.Property(e => e.ReportedByUserId).IsRequired();
        });

        // Configure Donation
        builder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DonorName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ResourceType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.DateDonated).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        });

        // Configure VolunteerTask
        builder.Entity<VolunteerTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.TaskDate).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        });
    }
}