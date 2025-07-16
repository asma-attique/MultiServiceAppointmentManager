using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MultiServiceAppointmentManager.ViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(AppointmentViewModel), nameof(ValidateDateInRange))]
        public DateTime Date { get; set; } = DateTime.Today;
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay;
        public string CustomerName { get; set; } = string.Empty;

        public int? ServiceId { get; set; }
        public IEnumerable<SelectListItem> Services { get; set; } = new List<SelectListItem>();

        public int? ProviderId { get; set; }
        public IEnumerable<SelectListItem> Providers { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> TimeSlots { get; set; } = new List<SelectListItem>();



        public List<Service> AvailableServices { get; set; } = new List<Service>();

        public static ValidationResult? ValidateDateInRange(DateTime date, ValidationContext context)
        {
            var today = DateTime.Today;
            var maxDate = today.AddMonths(2);

            if (date < today)
            {
                return new ValidationResult("Date cannot be in the past.");
            }

            if (date > maxDate)
            {
                return new ValidationResult("Date cannot be more than 2 months from today.");
            }

            return ValidationResult.Success;
        }
    }
}
