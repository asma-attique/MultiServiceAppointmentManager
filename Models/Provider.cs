using System.ComponentModel.DataAnnotations;

public class Provider
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public int YearsOfExperience { get; set; }

    public ICollection<Service> Services { get; set; } 
}
