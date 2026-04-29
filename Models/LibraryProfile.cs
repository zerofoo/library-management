using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class LibraryProfile
    {
        [Key]
        public int LibraryId { get; set; }

        [Required(ErrorMessage = "Library name is required")]
        [StringLength(100)]
        [Display(Name = "Library Name")]
        public string LibraryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Operating Hours")]
        public string? OperatingHours { get; set; }

        [Required(ErrorMessage = "Contact email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        [Display(Name = "Contact Phone")]
        public string? ContactPhone { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public int CreatedByUserId { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public virtual User? CreatedByUser { get; set; }
        
        public virtual BorrowingConfiguration? Configuration { get; set; }
    }
}
