using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiServiceAppointmentManager.Data;
using MultiServiceAppointmentManager.Models;
using MultiServiceAppointmentManager.ViewModels;

public class AppointmentsController : Controller
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    // 👉 Utility: Time slots helper
    private List<TimeSpan> GetTimeSlots()
    {
        var slots = new List<TimeSpan>();
        var start = new TimeSpan(10, 0, 0);
        var end = new TimeSpan(20, 0, 0);
        var lunchStart = new TimeSpan(14, 0, 0);
        var lunchEnd = new TimeSpan(15, 0, 0);

        for (var time = start; time < end; time = time.Add(TimeSpan.FromMinutes(30)))
        {
            if (time >= lunchStart && time < lunchEnd) continue;
            slots.Add(time);
        }

        return slots;
    }

    // GET: Appointments
    public async Task<IActionResult> Index(DateTime? date, int? providerId)
    {
        var query = _context.Appointments
            .Include(a => a.Provider)
            .Include(a => a.Service)
            .AsQueryable();

        if (date.HasValue)
        {
            query = query.Where(a => a.Date == date.Value.Date);
        }

        if (providerId.HasValue && providerId.Value > 0)
        {
            query = query.Where(a => a.ProviderId == providerId.Value);
        }

        // 1️⃣ Pull from DB first (TimeSpan ordering is not supported in SQLite)
        var appointmentsFromDb = await query.ToListAsync();

        // 2️⃣ Then do TimeSpan ordering in memory
        var orderedAppointments = appointmentsFromDb
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Time)
            .ToList();

        // Add Providers for dropdown
        ViewBag.Providers = await _context.Providers
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToListAsync();

        return View(orderedAppointments);
    }


    // 👉 GET: Appointments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Provider)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null) return NotFound();

        return View(appointment);
    }

    // 👉 GET: Appointments/Create
    public IActionResult Create()
    {
        var viewModel = new AppointmentViewModel
        {
            Services = _context.Services
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList(),

            Providers = new List<SelectListItem>(), // Populated dynamically

            TimeSlots = GetTimeSlots().Select(slot => new SelectListItem
            {
                Value = slot.ToString(@"hh\:mm"),
                Text = DateTime.Today.Add(slot).ToString("hh:mm tt")
            }).ToList()
        };

        return View(viewModel);
    }

    // 👉 POST: Appointments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var selectedService = await _context.Services.FindAsync(model.ServiceId);

            if (selectedService == null)
            {
                ModelState.AddModelError("ServiceId", "Selected service not found.");
                return View(model);
            }

            var appointment = new Appointment
            {
                Date = model.Date,
                Time = model.Time,
                CustomerName = CapitalizeFirstLetter(model.CustomerName),
                ProviderId = model.ProviderId ?? 0,
                ServiceId = model.ServiceId ?? 0,
                TotalCharges = selectedService.Charges
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Rebuild dropdowns if form errors
        model.Services = _context.Services
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();

        model.Providers = _context.Providers
            .Where(p => p.Services.Any(s => s.Id == model.ServiceId))
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();

        model.TimeSlots = GetTimeSlots().Select(slot => new SelectListItem
        {
            Value = slot.ToString(@"hh\:mm"),
            Text = DateTime.Today.Add(slot).ToString("hh:mm tt")
        }).ToList();

        return View(model);
    }

    // 👉 GET: Appointments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Provider)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null) return NotFound();

        var model = new AppointmentViewModel
        {
            Id = appointment.Id,
            Date = appointment.Date,
            Time = appointment.Time,
            CustomerName = appointment.CustomerName,
            ProviderId = appointment.ProviderId,
            ServiceId = appointment.ServiceId,

            Services = _context.Services
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList(),

            Providers = _context.Providers
                .Where(p => p.Services.Any(s => s.Id == appointment.ServiceId))
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),

            TimeSlots = GetTimeSlots().Select(slot => new SelectListItem
            {
                Value = slot.ToString(@"hh\:mm"),
                Text = DateTime.Today.Add(slot).ToString("hh:mm tt")
            }).ToList()
        };

        return View(model);
    }

    // 👉 POST: Appointments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AppointmentViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null) return NotFound();

            appointment.Date = model.Date;
            appointment.Time = model.Time;
            appointment.CustomerName = CapitalizeFirstLetter(model.CustomerName);
            appointment.ProviderId = model.ProviderId ?? 0;
            appointment.ServiceId = model.ServiceId ?? 0;

            var selectedService = await _context.Services.FindAsync(model.ServiceId);
            appointment.TotalCharges = selectedService?.Charges ?? 0;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Rebuild dropdowns
        model.Services = _context.Services
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();

        model.Providers = _context.Providers
            .Where(p => p.Services.Any(s => s.Id == model.ServiceId))
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();

        model.TimeSlots = GetTimeSlots().Select(slot => new SelectListItem
        {
            Value = slot.ToString(@"hh\:mm"),
            Text = DateTime.Today.Add(slot).ToString("hh:mm tt")
        }).ToList();

        return View(model);
    }

    // 👉 GET: Appointments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Provider)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null) return NotFound();

        return View(appointment);
    }

    // 👉 POST: Appointments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool AppointmentExists(int id) =>
        _context.Appointments.Any(e => e.Id == id);

    // 👉 Helper: Capitalize customer name
    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

    // 👉 AJAX: Get Providers for selected Service
    [HttpGet]
    public JsonResult GetProvidersByService(int serviceId)
    {
        var providers = _context.Providers
            .Where(p => p.Services.Any(s => s.Id == serviceId))
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

        return Json(providers);
    }
}
