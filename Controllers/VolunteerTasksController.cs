using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers;

[Authorize]
public class VolunteerTasksController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VolunteerTasksController> _logger;

    public VolunteerTasksController(ApplicationDbContext context, ILogger<VolunteerTasksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: VolunteerTasks
    public async Task<IActionResult> Index()
    {
        var tasks = await _context.VolunteerTasks
            .OrderBy(t => t.TaskDate)
            .ToListAsync();
        return View(tasks);
    }

    // GET: Available Tasks
    [Authorize(Roles = "Volunteer,Admin")]
    public async Task<IActionResult> Available()
    {
        var availableTasks = await _context.VolunteerTasks
            .Where(t => t.Status == "Open" && t.AssignedVolunteerId == null)
            .OrderBy(t => t.TaskDate)
            .ToListAsync();
        return View(availableTasks);
    }

    // GET: My Tasks
    [Authorize(Roles = "Volunteer,Admin")]
    public async Task<IActionResult> MyTasks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var myTasks = await _context.VolunteerTasks
            .Where(t => t.AssignedVolunteerId == userId)
            .OrderBy(t => t.TaskDate)
            .ToListAsync();
        return View(myTasks);
    }

    // GET: VolunteerTasks/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var volunteerTask = await _context.VolunteerTasks
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (volunteerTask == null)
        {
            return NotFound();
        }

        return View(volunteerTask);
    }

    // GET: VolunteerTasks/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: VolunteerTasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Title,Description,TaskDate,Location,RequiredSkills,EstimatedHours,Priority,Notes")] VolunteerTask volunteerTask)
    {
        if (ModelState.IsValid)
        {
            volunteerTask.Status = "Open";
            volunteerTask.CreatedDate = DateTime.Now;

            _context.Add(volunteerTask);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("New volunteer task created: {Title} scheduled for {TaskDate}", 
                volunteerTask.Title, volunteerTask.TaskDate);
            
            TempData["Success"] = "Volunteer task created successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(volunteerTask);
    }

    // POST: Sign up for task
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Volunteer,Admin")]
    public async Task<IActionResult> SignUp(int id)
    {
        var task = await _context.VolunteerTasks.FindAsync(id);
        if (task != null && task.Status == "Open" && task.AssignedVolunteerId == null)
        {
            task.AssignedVolunteerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            task.Status = "Assigned";
            
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("User {UserId} signed up for task {TaskTitle}", 
                task.AssignedVolunteerId, task.Title);
            
            TempData["Success"] = "You have successfully signed up for this task!";
        }
        else
        {
            TempData["Error"] = "Unable to sign up for this task.";
        }
        
        return RedirectToAction(nameof(Available));
    }

    // POST: Complete task
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Volunteer,Admin")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        var task = await _context.VolunteerTasks.FindAsync(id);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (task != null && (task.AssignedVolunteerId == userId || User.IsInRole("Admin")))
        {
            task.Status = "Completed";
            task.CompletionDate = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Task {TaskTitle} marked as completed by user {UserId}", 
                task.Title, userId);
            
            TempData["Success"] = "Task marked as completed!";
        }
        else
        {
            TempData["Error"] = "Unable to complete this task.";
        }
        
        return RedirectToAction(nameof(MyTasks));
    }

    // GET: VolunteerTasks/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
        if (volunteerTask == null)
        {
            return NotFound();
        }
        return View(volunteerTask);
    }

    // POST: VolunteerTasks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TaskDate,AssignedVolunteerId,Status,Location,RequiredSkills,EstimatedHours,Priority,CreatedDate,CompletionDate,Notes")] VolunteerTask volunteerTask)
    {
        if (id != volunteerTask.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(volunteerTask);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Volunteer task updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerTaskExists(volunteerTask.Id))
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
        return View(volunteerTask);
    }

    // GET: VolunteerTasks/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var volunteerTask = await _context.VolunteerTasks
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (volunteerTask == null)
        {
            return NotFound();
        }

        return View(volunteerTask);
    }

    // POST: VolunteerTasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
        if (volunteerTask != null)
        {
            _context.VolunteerTasks.Remove(volunteerTask);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Volunteer task deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool VolunteerTaskExists(int id)
    {
        return _context.VolunteerTasks.Any(e => e.Id == id);
    }
}