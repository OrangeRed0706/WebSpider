using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSpider.Context.Entities;
//[Table("Post")]
public class Post
{
    [Key]
    public string? Title { get; set; }
    public string? Describe { get; set; }
    public string? Link { get; set; }
    public string? ImageUri { get; set; }
    public DateTime Time { get; set; }

}