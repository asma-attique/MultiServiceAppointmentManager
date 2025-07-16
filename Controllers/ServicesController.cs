using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using MultiServiceAppointmentManager.Data;

public class ServicesController : Controller
{
    private readonly AppDbContext _context;

    public ServicesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Services
    public async Task<IActionResult> Index()
    {
        var services = await _context.Services.ToListAsync();
        return View(services);
    }

    // GET: Services/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Services/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Charges")] Service service)
    {
        service.Name = CapitalizeFirstLetter(service.Name);
        if (ModelState.IsValid)
        {
            _context.Add(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(service);
    }

    // GET: Services/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();

        return View(service);
    }

    // POST: Services/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Charges")] Service service)
    {
        if (id != service.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                service.Name = CapitalizeFirstLetter(service.Name);
                _context.Update(service);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Services.Any(e => e.Id == service.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(service);
    }

    // GET: Services/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var service = await _context.Services
            .FirstOrDefaultAsync(m => m.Id == id);

        if (service == null) return NotFound();

        return View(service);
    }

    // POST: Services/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var service = await _context.Services.FindAsync(id);
         if (service == null)
        {
            return NotFound();
        }
        try
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Service deleted successfully!";
        }
        catch (DbUpdateException)
        {
            // If there’s a foreign key conflict
            TempData["ErrorMessage"] = "Cannot delete this service because it has related providers or Appointments.";
        }
        return RedirectToAction(nameof(Index));
    }
    
     private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
}
