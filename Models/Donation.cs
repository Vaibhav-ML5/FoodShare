
using System;
using System.ComponentModel.DataAnnotations;

namespace FoodShare.Models
{
    public class Donation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the food title.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a food type.")]
        [StringLength(50)]
        public string FoodType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the quantity.")]
        [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10000.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter the pickup location.")]
        [StringLength(200)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the donor name.")]
        [StringLength(100)]
        public string DonorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a contact number.")]
        [Phone(ErrorMessage = "Please enter a valid contact number.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a pickup deadline.")]
        public DateTime PickupDeadline { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Available";
[StringLength(100)]
public string? ClaimedBy { get; set; }

public DateTime? ClaimedAt { get; set; }

public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
