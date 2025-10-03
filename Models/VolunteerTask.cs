using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models;

public class VolunteerTask
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Task title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [Display(Name = "Task Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Task description is required")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [Display(Name = "Task Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Task Date")]
    [DataType(DataType.DateTime)]
    public DateTime TaskDate { get; set; } = DateTime.Now.AddDays(1);

    [Display(Name = "Assigned Volunteer")]
    public string? AssignedVolunteerId { get; set; }

    [Required]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Open";

    [Display(Name = "Location")]
    [StringLength(500, ErrorMessage = "Location cannot exceed 500 characters")]
    public string? Location { get; set; }

    [Display(Name = "Required Skills")]
    [StringLength(500, ErrorMessage = "Required skills cannot exceed 500 characters")]
    public string? RequiredSkills { get; set; }

    [Display(Name = "Estimated Hours")]
    [Range(0.5, 24, ErrorMessage = "Estimated hours must be between 0.5 and 24")]
    public double? EstimatedHours { get; set; }

    [Display(Name = "Priority Level")]
    public string Priority { get; set; } = "Medium";

    [Display(Name = "Created Date")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Display(Name = "Completion Date")]
    [DataType(DataType.DateTime)]
    public DateTime? CompletionDate { get; set; }

    [Display(Name = "Notes")]
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }
}