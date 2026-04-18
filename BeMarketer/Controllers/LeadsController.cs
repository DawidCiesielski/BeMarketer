using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BeMarketer.Data;
using BeMarketer.Models;

namespace BeMarketer.Controllers
{
    [Authorize] // Wymaga zalogowania dla wszystkich akcji w tym kontrolerze
    public class LeadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeadsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Leads
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            // Przygotowujemy zapytanie do bazy
            IQueryable<Lead> leadsQuery = _context.Lead.Include(l => l.ApplicationUser);

            // Jeśli to nie jest Admin, filtrujemy wyniki tylko do Leadów przypisanych do tego użytkownika
            if (!isAdmin)
            {
                leadsQuery = leadsQuery.Where(l => l.ApplicationUserId == userId);
            }

            return View(await leadsQuery.ToListAsync());
        }

        // GET: Leads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Lead
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lead == null)
            {
                return NotFound();
            }

            // Ochrona: Jeśli nie jesteś adminem i to nie jest Twój lead -> Brak dostępu
            if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(lead);
        }

        // GET: Leads/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName");
            }
            return View();
        }

        // POST: Leads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Address,Description,Status,CreatedAt,ApplicationUserId")] Lead lead)
        {
            if (!User.IsInRole("Admin"))
            {
                lead.ApplicationUserId = _userManager.GetUserId(User);
                ModelState.Remove("ApplicationUserId");
            }

            if (lead.CreatedAt == default)
            {
                lead.CreatedAt = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                _context.Add(lead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Admin"))
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", lead.ApplicationUserId);
            }
            return View(lead);
        }

        // GET: Leads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Lead.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            // Ochrona: Zwykły użytkownik nie może wejść w edycję cudzego leada
            if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            if (User.IsInRole("Admin"))
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", lead.ApplicationUserId);
            }
            return View(lead);
        }

        // POST: Leads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Address,Description,Status,CreatedAt,ApplicationUserId")] Lead lead)
        {
            if (id != lead.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            // Sprawdzamy oryginalnego leada w bazie (AsNoTracking żeby nie blokować późniejszego Update)
            var originalLead = await _context.Lead.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

            if (originalLead == null)
            {
                return NotFound();
            }

            // Ochrona 1: Próba edycji nieswojego leada przez POST
            if (!isAdmin && originalLead.ApplicationUserId != userId)
            {
                return Forbid();
            }

            // Ochrona 2: Zwykły użytkownik nie może zmienić przypisania leada, nadpisujemy na jego własne ID
            if (!isAdmin)
            {
                lead.ApplicationUserId = userId;
                ModelState.Remove("ApplicationUserId");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeadExists(lead.Id))
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

            if (isAdmin)
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", lead.ApplicationUserId);
            }
            return View(lead);
        }

        // GET: Leads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Lead
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lead == null)
            {
                return NotFound();
            }

            // Ochrona przed usunięciem cudzego leada
            if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(lead);
        }

        // POST: Leads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lead = await _context.Lead.FindAsync(id);
            if (lead != null)
            {
                // Ostatnia kontrola w POST Delete przed fizycznym skasowaniem
                if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
                {
                    return Forbid();
                }

                _context.Lead.Remove(lead);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LeadExists(int id)
        {
            return _context.Lead.Any(e => e.Id == id);
        }
    }
}