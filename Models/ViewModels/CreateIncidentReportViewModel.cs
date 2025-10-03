using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models.ViewModels;

public class CreateIncidentReportViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [Display(Name = "Incident Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    [Display(Name = "Incident Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required")]
    [StringLength(500, ErrorMessage = "Location cannot exceed 500 characters")]
    [Display(Name = "Incident Location")]
    public string Location { get; set; } = string.Empty;

    [Display(Name = "Priority Level")]
    public string Priority { get; set; } = "Medium";
}