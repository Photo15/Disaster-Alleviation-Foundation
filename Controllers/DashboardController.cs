using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models.ViewModels;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.IsInRole("Admin") ? "Admin" : 
                      User.IsInRole("Volunteer") ? "Volunteer" :
                      User.IsInRole("Donor") ? "Donor" : "RegularUser";

        var viewModel = new DashboardViewModel
        {
            TotalIncidents = await _context.IncidentReports.CountAsync(),
            PendingDonations = await _context.Donations.CountAsync(d => d.Status == "Pending"),
            OpenVolunteerTasks = await _context.VolunteerTasks.CountAsync(t => t.Status == "Open"),
            RecentIncidents = await _context.IncidentReports
                .OrderByDescending(i => i.DateReported)
                .Take(5)
                .ToListAsync(),
            RecentDonations = await _context.Donations
                .OrderByDescending(d => d.DateDonated)
                .Take(5)
                .ToListAsync(),
            UpcomingTasks = await _context.VolunteerTasks
                .Where(t => t.TaskDate >= DateTime.Today && t.Status != "Completed")
                .OrderBy(t => t.TaskDate)
                .Take(5)
                .ToListAsync()
        };

        // Personalize dashboard based on user role
        if (userRole == "Volunteer")
        {
            viewModel.UpcomingTasks = await _context.VolunteerTasks
                .Where(t => t.AssignedVolunteerId == userId && t.Status != "Completed")
                .OrderBy(t => t.TaskDate)
                .Take(5)
                .ToListAsync();
        }
        else if (userRole == "Donor")
        {
            viewModel.RecentDonations = await _context.Donations
                .Where(d => d.DonorUserId == userId)
                .OrderByDescending(d => d.DateDonated)
                .Take(5)
                .ToListAsync();
        }

        ViewBag.UserRole = userRole;
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Admin()
    {
        var viewModel = new DashboardViewModel
        {
            TotalIncidents = await _context.IncidentReports.CountAsync(),
            PendingDonations = await _context.Donations.CountAsync(d => d.Status == "Pending"),
            OpenVolunteerTasks = await _context.VolunteerTasks.CountAsync(t => t.Status == "Open"),
            RecentIncidents = await _context.IncidentReports
                .OrderByDescending(i => i.DateReported)
                .Take(10)
                .ToListAsync(),
            RecentDonations = await _context.Donations
                .Where(d => d.Status == "Pending")
                .OrderByDescending(d => d.DateDonated)
                .Take(10)
                .ToListAsync(),
            UpcomingTasks = await _context.VolunteerTasks
                .Where(t => t.Status == "Open")
                .OrderBy(t => t.TaskDate)
                .Take(10)
                .ToListAsync()
        };

        return View(viewModel);
    }
}