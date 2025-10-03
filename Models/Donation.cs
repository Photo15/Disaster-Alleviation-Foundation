using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models;

public class Donation
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Donor name is required")]
    [StringLength(100, ErrorMessage = "Donor name cannot exceed 100 characters")]
    [Display(Name = "Donor Name")]
    public string DonorName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Resource type is required")]
    [StringLength(100, ErrorMessage = "Resource type cannot exceed 100 characters")]
    [Display(Name = "Resource Type")]
    public string ResourceType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; }

    [Required]
    [Display(Name = "Date Donated")]
    [DataType(DataType.DateTime)]
    public DateTime DateDonated { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Pending";

    [Display(Name = "Donor Email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? DonorEmail { get; set; }

    [Display(Name = "Donor Phone")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? DonorPhone { get; set; }

    [Display(Name = "Delivery Address")]
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? DeliveryAddress { get; set; }

    [Display(Name = "Notes")]
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }

    [Display(Name = "Donor User ID")]
    public string? DonorUserId { get; set; }
}