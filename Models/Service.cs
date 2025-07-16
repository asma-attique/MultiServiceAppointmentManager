using System.ComponentModel.DataAnnotations;
using MultiServiceAppointmentManager.Models;

public class Service
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Charges { get; set; }
    public List<Provider> Providers { get; set; } = new();
    
}
