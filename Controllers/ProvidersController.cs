using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using MultiServiceAppointmentManager.Data;

public class ProvidersController : Controller
{
    private readonly AppDbContext _context;

    public ProvidersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Providers
    public async Task<IActionResult> Index()
    {
        var providers = await _context.Providers.Include(p => p.Services).ToListAsync();
        return View(providers);
    }

    // GET: Providers/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new ProviderViewModel
        {
            AvailableServices = await _context.Services.ToListAsync()
        };
        return View(viewModel);
    }


    // POST: Providers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProviderViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            viewModel.Name = CapitalizeFirstLetter(viewModel.Name);
            var provider = new Provider
            {
                Name = viewModel.Name,
                YearsOfExperience = viewModel.YearsOfExperience,
                // If you use Many-to-Many:
                Services = await _context.Services
                    .Where(s => viewModel.SelectedServiceIds.Contains(s.Id))
                    .ToListAsync()
            };

            _context.Add(provider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // If validation fails, reload AvailableServices!
        viewModel.AvailableServices = await _context.Services.ToListAsync();
        return View(viewModel);
    }





    // GET: Providers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var provider = await _context.Providers
            .Include(p => p.Services)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (provider == null) return NotFound();

        var viewModel = new ProviderViewModel
        {
            Id = provider.Id,
            Name = provider.Name,
            YearsOfExperience = provider.YearsOfExperience,
            SelectedServiceIds = provider.Services.Select(s => s.Id).ToList(),
            AvailableServices = _context.Services.ToList()
        };

        return View(viewModel);
    }

    // POST: Providers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProviderViewModel viewModel)
    {
        if (id != viewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var provider = await _context.Providers
                .Include(p => p.Services)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (provider == null) return NotFound();

            provider.Name = CapitalizeFirstLetter(viewModel.Name);
            provider.YearsOfExperience = viewModel.YearsOfExperience;

            // ✅ Update selected services
            provider.Services.Clear();
            provider.Services = _context.Services
                .Where(s => viewModel.SelectedServiceIds.Contains(s.Id))
                .ToList();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Repopulate if validation failed
        viewModel.AvailableServices = _context.Services.ToList();
        return View(viewModel);
    }


    private bool ProviderExists(int id)
    {
        return _context.Providers.Any(e => e.Id == id);
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var provider = await _context.Providers
            .Include(p => p.Services)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (provider == null) return NotFound();

        return View(provider);
    }


    // GET: Providers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var provider = await _context.Providers
            .Include(p => p.Services)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (provider == null) return NotFound();

        return View(provider);
    }


    // POST: Providers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var provider = await _context.Providers.FindAsync(id);

        if (provider == null)
        {
            return NotFound();
        }
        try
        {
            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Provider deleted successfully!";

        }
        catch (DbUpdateException)
        {
            // If there’s a foreign key conflict
            TempData["ErrorMessage"] = "Cannot delete this provider because it has related Appointments.";
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

