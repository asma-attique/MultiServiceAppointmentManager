using System;
using System.ComponentModel.DataAnnotations;

namespace MultiServiceAppointmentManager.Models
{
    public class Appointment
    {
        public int Id { get; set; } // Acts as AppointmentNumber

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        private string _customerName = string.Empty;

        [Required]
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _customerName = char.ToUpper(value[0]) + value.Substring(1).ToLower();
                else
                    _customerName = value;
            }
        }

        // Provider relationship
        [Required]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; } = null!;

        // ✅ Single Service per Appointment
        [Required]
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        // Charges directly related to the selected Service
        [DataType(DataType.Currency)]
        public decimal TotalCharges { get; set; }
    }
}
