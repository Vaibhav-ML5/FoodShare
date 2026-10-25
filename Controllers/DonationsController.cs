
using FoodShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShare.Controllers
{
    public class DonationsController : Controller
    {
        private readonly FoodShareContext _context;

        public DonationsController(FoodShareContext context)
        {
            _context = context;
        }

       public async Task<IActionResult> Index(
    string? search,
    string? foodType,
    string? status)
        {
            var query = _context.Donations.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(d =>
                    d.Title.Contains(term) ||
                    d.PickupLocation.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(foodType))
                query = query.Where(d => d.FoodType == foodType);
            if (!string.IsNullOrWhiteSpace(status))
{
    query = query.Where(d => d.Status == status);
}

ViewBag.Status = status;
            if (!string.IsNullOrWhiteSpace(status))
    query = query.Where(d => d.Status == status);

ViewBag.Status = status;


            ViewBag.Search = search;
            ViewBag.FoodType = foodType;
            ViewBag.Status = status;
            var donations = await query
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return View(donations);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation donation)
        {
            if (donation.PickupDeadline <= DateTime.Now)
                ModelState.AddModelError(
                    nameof(donation.PickupDeadline),
                    "Pickup deadline must be in the future.");

            if (!ModelState.IsValid)
                return View(donation);

            donation.Status = "Available";
            donation.CreatedAt = DateTime.Now;

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your food donation has been added!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            return donation == null ? NotFound() : View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Claim(int id, string claimedBy)
        {
            if (string.IsNullOrWhiteSpace(claimedBy) ||
                claimedBy.Trim().Length > 100)
            {
                TempData["Error"] = "Enter a name of up to 100 characters.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            if (donation.Status != "Available" ||
                donation.PickupDeadline <= DateTime.Now)
            {
                TempData["Error"] = "This donation is no longer available.";
                return RedirectToAction(nameof(Details), new { id });
            }

            donation.ClaimedBy = claimedBy.Trim();
            donation.ClaimedAt = DateTime.Now;
            donation.Status = "Claimed";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Donation claimed successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            if (donation.Status != "Claimed")
            {
                TempData["Error"] = "Only claimed donations can be completed.";
                return RedirectToAction(nameof(Details), new { id });
            }

            donation.Status = "Completed";
            donation.CompletedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Donation marked as collected!";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Dashboard()
        {
            var donations = await _context.Donations
                .AsNoTracking()
                .ToListAsync();

            ViewBag.TotalDonations = donations.Count;
            ViewBag.AvailableDonations =
                donations.Count(d => d.Status == "Available");
            ViewBag.ClaimedDonations =
                donations.Count(d => d.Status == "Claimed");
            ViewBag.CompletedDonations =
                donations.Count(d => d.Status == "Completed");

            var recent = donations
                .OrderByDescending(d => d.CreatedAt)
                .Take(5)
                .ToList();

            return View(recent);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            if (donation.Status != "Available")
            {
                TempData["Error"] = "Only available donations can be edited.";
                return RedirectToAction(nameof(Index));
            }

            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Donation donation)
        {
            if (id != donation.Id)
                return NotFound();

            var existing = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (existing == null)
                return NotFound();

            if (existing.Status != "Available")
            {
                TempData["Error"] = "Only available donations can be edited.";
                return RedirectToAction(nameof(Details), new { id });
            }

            if (donation.PickupDeadline <= DateTime.Now)
                ModelState.AddModelError(
                    nameof(donation.PickupDeadline),
                    "Pickup deadline must be in the future.");

            if (!ModelState.IsValid)
                return View(donation);

            existing.Title = donation.Title;
            existing.FoodType = donation.FoodType;
            existing.Quantity = donation.Quantity;
            existing.PickupLocation = donation.PickupLocation;
            existing.DonorName = donation.DonorName;
            existing.ContactNumber = donation.ContactNumber;
            existing.PickupDeadline = donation.PickupDeadline;
            existing.Description = donation.Description;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Donation updated successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            if (donation.Status != "Available")
            {
                TempData["Error"] = "Only available donations can be deleted.";
                return RedirectToAction(nameof(Index));
            }

            return View(donation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            if (donation.Status != "Available")
            {
                TempData["Error"] = "Only available donations can be deleted.";
                return RedirectToAction(nameof(Index));
            }

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Donation deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
