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
    [Authorize]
    public class LeadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeadsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            IQueryable<Lead> leadsQuery = _context.Lead.Include(l => l.ApplicationUser);

            if (!isAdmin)
            {
                leadsQuery = leadsQuery.Where(l => l.ApplicationUserId == userId);
            }

            return View(await leadsQuery.ToListAsync());
        }

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

            if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(lead);
        }

        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Address,Description,Status,CreatedAt,ApplicationUserId")] Lead lead)
        {
            if (!User.IsInRole("Admin"))
            {
                lead.ApplicationUserId = _userManager.GetUserId(User);
                ModelState.Remove("ApplicationUserId");
            }

            lead.CreatedAt = DateTime.Now;

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

            if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(LeadStatus)), lead.Status);

            if (User.IsInRole("Admin"))
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", lead.ApplicationUserId);
            }
            return View(lead);
        }

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

            var originalLead = await _context.Lead.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

            if (originalLead == null)
            {
                return NotFound();
            }
            
            if (!isAdmin && originalLead.ApplicationUserId != userId)
            {
                return Forbid();
            }

            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(LeadStatus)), lead.Status); 

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
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(LeadStatus)));
            if (isAdmin)
            {
                ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", lead.ApplicationUserId);
            }
            return View(lead);
        }

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
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(LeadStatus)));

            if (!User.IsInRole("Admin") && lead.ApplicationUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(lead);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lead = await _context.Lead.FindAsync(id);
            if (lead != null)
            {

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