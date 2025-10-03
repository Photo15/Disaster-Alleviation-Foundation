using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviationFoundation.Models.ViewModels;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers;

[Authorize]
public class IncidentReportsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IncidentReportsController> _logger;

    public IncidentReportsController(ApplicationDbContext context, ILogger<IncidentReportsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: IncidentReports
    public async Task<IActionResult> Index()
    {
        var incidents = await _context.IncidentReports
            .OrderByDescending(i => i.DateReported)
            .ToListAsync();
        return View(incidents);
    }

    // GET: IncidentReports/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incidentReport = await _context.IncidentReports
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (incidentReport == null)
        {
            return NotFound();
        }

        return View(incidentReport);
    }

    // GET: IncidentReports/Create
    public IActionResult Create()
    {
        return View(new CreateIncidentReportViewModel());
    }

    // POST: IncidentReports/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateIncidentReportViewModel model)
    {
        _logger.LogInformation("Create POST action called");
        _logger.LogInformation($"Model received - Title: {model.Title}, Description: {model.Description}, Location: {model.Location}, Priority: {model.Priority}");
        
        // Log all ModelState errors
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState)
            {
                foreach (var subError in error.Value.Errors)
                {
                    _logger.LogError($"ModelState Error - Key: {error.Key}, Error: {subError.ErrorMessage}");
                }
            }
            return View(model);
        }

        try
        {
            var incidentReport = new IncidentReport
            {
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                Priority = model.Priority,
                ReportedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                DateReported = DateTime.Now,
                Status = "Open"
            };

            _logger.LogInformation($"Adding incident to context: {incidentReport.Title}");
            _context.Add(incidentReport);
            
            _logger.LogInformation("Calling SaveChangesAsync");
            var result = await _context.SaveChangesAsync();
            _logger.LogInformation($"SaveChangesAsync returned: {result} changes");
            
            _logger.LogInformation("New incident report created: {Title} by user {UserId}", 
                incidentReport.Title, incidentReport.ReportedByUserId);
            
            TempData["Success"] = "Incident report submitted successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving incident report");
            ModelState.AddModelError("", "An error occurred while saving the incident report. Please try again.");
            return View(model);
        }
    }

    // GET: IncidentReports/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incidentReport = await _context.IncidentReports.FindAsync(id);
        if (incidentReport == null)
        {
            return NotFound();
        }
        return View(incidentReport);
    }

    // POST: IncidentReports/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,DateReported,ReportedByUserId,Status,Priority")] IncidentReport incidentReport)
    {
        if (id != incidentReport.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(incidentReport);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Incident report updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentReportExists(incidentReport.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(incidentReport);
    }

    // GET: IncidentReports/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incidentReport = await _context.IncidentReports
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (incidentReport == null)
        {
            return NotFound();
        }

        return View(incidentReport);
    }

    // POST: IncidentReports/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var incidentReport = await _context.IncidentReports.FindAsync(id);
        if (incidentReport != null)
        {
            _context.IncidentReports.Remove(incidentReport);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Incident report deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool IncidentReportExists(int id)
    {
        return _context.IncidentReports.Any(e => e.Id == id);
    }
}