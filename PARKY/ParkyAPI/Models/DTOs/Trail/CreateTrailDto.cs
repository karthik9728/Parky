using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.DTOs.Trail
{
    public class CreateTrailDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
    }
}
