using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    public class FlightDto
    {
        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public string SeatClass { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public class FlightCreateUpdateDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FlightNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Origin { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        [StringLength(50)]
        public string SeatClass { get; set; } = string.Empty;
    }
}
