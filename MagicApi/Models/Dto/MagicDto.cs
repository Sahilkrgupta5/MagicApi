using System.ComponentModel.DataAnnotations;

namespace MagicApi.Models.Dto
{
    public class MagicDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}
