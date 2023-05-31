using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server;

[Table("ArtWork")]
public class ArtWork
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public string ImageThumbnailUrl { get; set; } = null!;

    [Required]
    public bool IsFeatured { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}