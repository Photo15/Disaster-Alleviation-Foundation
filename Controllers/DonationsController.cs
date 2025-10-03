using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers;

[Authorize]
public class DonationsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DonationsController> _logger;

    public DonationsController(ApplicationDbContext context, ILogger<DonationsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Donations
    public async Task<IActionResult> Index()
    {
        var donations = await _context.Donations
            .OrderByDescending(d => d.DateDonated)
            .ToListAsync();
        return View(donations);
    }

    // GET: My Donations
    public async Task<IActionResult> MyDonations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var donations = await _context.Donations
            .Where(d => d.DonorUserId == userId)
            .OrderByDescending(d => d.DateDonated)
            .ToListAsync();
        return View(donations);
    }

    // GET: Donations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donation = await _context.Donations
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (donation == null)
        {
            return NotFound();
        }

        return View(donation);
    }

    // GET: Donations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Donations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DonorName,ResourceType,Quantity,DonorEmail,DonorPhone,DeliveryAddress,Notes")] Donation donation)
    {
        if (ModelState.IsValid)
        {
            donation.DonorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            donation.DateDonated = DateTime.Now;
            donation.Status = "Pending";

            _context.Add(donation);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("New donation created: {ResourceType} x{Quantity} by {DonorName}", 
                donation.ResourceType, donation.Quantity, donation.DonorName);
            
            TempData["Success"] = "Thank you for your donation! It has been submitted for review.";
            return RedirectToAction(nameof(MyDonations));
        }
        return View(donation);
    }

    // GET: Donations/Edit/5
    [Authorize(Roles = "Admin,Donor")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donation = await _context.Donations.FindAsync(id);
        if (donation == null)
        {
            return NotFound();
        }

        // Check if user owns this donation or is admin
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Admin") && donation.DonorUserId != userId)
        {
            return Forbid();
        }

        return View(donation);
    }

    // POST: Donations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Donor")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,DonorName,ResourceType,Quantity,DateDonated,Status,DonorEmail,DonorPhone,DeliveryAddress,Notes,DonorUserId")] Donation donation)
    {
        if (id != donation.Id)
        {
            return NotFound();
        }

        // Check if user owns this donation or is admin
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Admin") && donation.DonorUserId != userId)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(donation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Donation updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(donation.Id))
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
        return View(donation);
    }

    // POST: Update Status (Admin only)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var donation = await _context.Donations.FindAsync(id);
        if (donation != null)
        {
            donation.Status = status;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Donation status updated to {status}";
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Donations/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donation = await _context.Donations
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (donation == null)
        {
            return NotFound();
        }

        return View(donation);
    }

    // POST: Donations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var donation = await _context.Donations.FindAsync(id);
        if (donation != null)
        {
            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Donation deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool DonationExists(int id)
    {
        return _context.Donations.Any(e => e.Id == id);
    }
}