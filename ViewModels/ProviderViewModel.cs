public class ProviderViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int YearsOfExperience { get; set; }
    public List<Service>? AvailableServices { get; set; } = new();
    public List<int>? SelectedServiceIds { get; set; } = new();
}