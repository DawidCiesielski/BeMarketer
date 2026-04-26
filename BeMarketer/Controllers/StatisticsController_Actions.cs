using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BeMarketer.Data;
using BeMarketer.Models;
using BeMarketer.ViewModels;

namespace BeMarketer.Controllers
{
    [Authorize] // Wymaga zalogowania
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Statistics
        public async Task<IActionResult> Index(
            int? year,
            int? month,
            int? compareYear,
            int? compareMonth)
        {
            // Tylko Admin widzi statystyki wszystkich
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var currentYear = DateTime.Now.Year;
            var selectedYear = year ?? currentYear;
            var selectedMonth = month; // null = cały rok

            // Dostępne lata (od pierwszego leada do teraz)
            var availableYears = await _context.Lead
                .Select(l => l.CreatedAt.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

            if (!availableYears.Contains(currentYear))
                availableYears.Insert(0, currentYear);

            // ── Główny okres ──────────────────────────────────────────────
            var currentQuery = BuildPeriodQuery(selectedYear, selectedMonth);
            var currentPeriodStats = await GetPeriodStats(currentQuery);
            var employeeStats = await GetEmployeeStats(currentQuery);

            // Dane do wykresu
            List<DailyLeadCount> dailyData = new();
            List<(int Month, int Count)> monthlyData = new();

            if (selectedMonth.HasValue)
            {
                dailyData = await GetDailyData(selectedYear, selectedMonth.Value);
            }
            else
            {
                monthlyData = await GetMonthlyData(selectedYear);
            }

            // ── Okres porównawczy ─────────────────────────────────────────
            PeriodStats? comparePeriodStats = null;
            List<DailyLeadCount> compareDailyData = new();
            List<(int Month, int Count)> compareMonthlyData = new();

            if (compareYear.HasValue)
            {
                var compareQuery = BuildPeriodQuery(compareYear.Value, compareMonth);
                comparePeriodStats = await GetPeriodStats(compareQuery);

                if (compareMonth.HasValue)
                    compareDailyData = await GetDailyData(compareYear.Value, compareMonth.Value);
                else
                    compareMonthlyData = await GetMonthlyData(compareYear.Value);
            }

            var vm = new StatisticsViewModel
            {
                SelectedYear = selectedYear,
                SelectedMonth = selectedMonth,
                CompareYear = compareYear,
                CompareMonth = compareMonth,
                CurrentPeriodStats = currentPeriodStats,
                ComparePeriodStats = comparePeriodStats,
                TopEmployee = employeeStats.FirstOrDefault(),
                EmployeeRanking = employeeStats,
                DailyData = dailyData,
                CompareDailyData = compareDailyData,
                MonthlyData = monthlyData,
                CompareMonthlyData = compareMonthlyData,
                AvailableYears = availableYears
            };

            return View(vm);
        }

        // ── Metody pomocnicze ─────────────────────────────────────────────

        private IQueryable<Lead> BuildPeriodQuery(int year, int? month)
        {
            var query = _context.Lead.AsQueryable();
            query = query.Where(l => l.CreatedAt.Year == year);
            if (month.HasValue)
                query = query.Where(l => l.CreatedAt.Month == month.Value);
            return query;
        }

        private async Task<PeriodStats> GetPeriodStats(IQueryable<Lead> query)
        {
            return new PeriodStats
            {
                TotalLeads     = await query.CountAsync(),
                NewLeads       = await query.CountAsync(l => l.Status == LeadStatus.New),
                ContactedLeads = await query.CountAsync(l => l.Status == LeadStatus.Contacted),
                QualifiedLeads = await query.CountAsync(l => l.Status == LeadStatus.Qualified),
                LostLeads      = await query.CountAsync(l => l.Status == LeadStatus.Lost),
            };
        }

        private async Task<List<EmployeeStats>> GetEmployeeStats(IQueryable<Lead> query)
        {
            return await query
                .Include(l => l.ApplicationUser)
                .GroupBy(l => new { l.ApplicationUserId, l.ApplicationUser!.UserName })
                .Select(g => new EmployeeStats
                {
                    UserId         = g.Key.ApplicationUserId!,
                    UserName       = g.Key.UserName!,
                    LeadCount      = g.Count(),
                    NewLeads       = g.Count(l => l.Status == LeadStatus.New),
                    ContactedLeads = g.Count(l => l.Status == LeadStatus.Contacted),
                    QualifiedLeads = g.Count(l => l.Status == LeadStatus.Qualified),
                    LostLeads      = g.Count(l => l.Status == LeadStatus.Lost),
                })
                .OrderByDescending(e => e.QualifiedLeads)
                .ThenByDescending(e => e.LeadCount)
                .ToListAsync();
        }

        private async Task<List<DailyLeadCount>> GetDailyData(int year, int month)
        {
            return await _context.Lead
                .Where(l => l.CreatedAt.Year == year && l.CreatedAt.Month == month)
                .GroupBy(l => l.CreatedAt.Date)
                .Select(g => new DailyLeadCount { Date = g.Key, Count = g.Count() })
                .OrderBy(d => d.Date)
                .ToListAsync();
        }

        private async Task<List<(int Month, int Count)>> GetMonthlyData(int year)
        {
            var raw = await _context.Lead
                .Where(l => l.CreatedAt.Year == year)
                .GroupBy(l => l.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(g => g.Month)
                .ToListAsync();

            return raw.Select(r => (r.Month, r.Count)).ToList();
        }
    }
}
