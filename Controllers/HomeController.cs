using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiServiceAppointmentManager.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiServiceAppointmentManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);

            // 1️⃣ Appointments Today
            var todaysAppointments = await _context.Appointments
                .Where(a => a.Date == today)
                .Include(a => a.Provider)
                .Include(a => a.Service)
                .ToListAsync();
            var totalChargesToday = todaysAppointments.Sum(a => a.Service.Charges);

            // 2️⃣ Total Charges This Week
            var weekAppointments = await _context.Appointments
                .Where(a => a.Date >= weekStart && a.Date <= weekEnd)
                .Include(a => a.Service)
                .ToListAsync();
            var totalChargesThisWeek = weekAppointments.Sum(a => a.Service.Charges);

            // 3️⃣ Top Providers This Week
            var topProviders = await _context.Appointments
                .Where(a => a.Date >= weekStart && a.Date <= weekEnd)
                .Include(a => a.Provider)
                .GroupBy(a => a.Provider.Name)
                .Select(g => new { ProviderName = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            // 4️⃣ Top Services This Week
            var topServices = await _context.Appointments
                .Where(a => a.Date >= weekStart && a.Date <= weekEnd)
                .Include(a => a.Service)
                .GroupBy(a => a.Service.Name)
                .Select(g => new { ServiceName = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            // 5️⃣ Upcoming Appointments — fix: do TimeSpan sort in memory
            var upcomingAppointmentsRaw = await _context.Appointments
                .Where(a => a.Date >= today)
                .Include(a => a.Provider)
                .Include(a => a.Service)
                .OrderBy(a => a.Date)   // ✅ Date is fine in DB
                .ToListAsync();         // ✅ Bring to memory

            var upcomingAppointments = upcomingAppointmentsRaw
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)    // ✅ TimeSpan ordering in memory
                .Take(5)
                .ToList();

            // ✅ ViewBags
            ViewBag.TotalAppointmentsToday = todaysAppointments.Count;
            ViewBag.TotalChargesToday = totalChargesToday;
            ViewBag.TotalChargesThisWeek = totalChargesThisWeek;
            ViewBag.TopProviders = topProviders;
            ViewBag.TopServices = topServices;
            ViewBag.UpcomingAppointments = upcomingAppointments;

            // For Chart.js
            ViewBag.TopProvidersNames = topProviders.Select(p => p.ProviderName).ToList();
            ViewBag.TopProvidersCounts = topProviders.Select(p => p.Count).ToList();
            ViewBag.TopServicesNames = topServices.Select(p => p.ServiceName).ToList();
            ViewBag.TopServicesCounts = topServices.Select(p => p.Count).ToList();

            return View();
        }

    }
}
