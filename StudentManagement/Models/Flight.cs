using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Flight
    {
        [Key]
        public Guid FlightId { get; set; } = Guid.NewGuid();

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

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
