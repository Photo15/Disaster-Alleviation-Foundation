using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models.ViewModels;

public class DashboardViewModel
{
    public int TotalIncidents { get; set; }
    public int PendingDonations { get; set; }
    public int OpenVolunteerTasks { get; set; }
    public List<IncidentReport> RecentIncidents { get; set; } = new();
    public List<Donation> RecentDonations { get; set; } = new();
    public List<VolunteerTask> UpcomingTasks { get; set; } = new();
}

public class UserProfileViewModel
{
    [Display(Name = "User Name")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Full Name")]
    [StringLength(100)]
    public string? FullName { get; set; }

    [Display(Name = "Phone Number")]
    [Phone]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Role")]
    public string Role { get; set; } = string.Empty;
}