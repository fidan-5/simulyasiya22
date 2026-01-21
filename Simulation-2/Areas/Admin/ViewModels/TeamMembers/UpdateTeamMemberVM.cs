using System.ComponentModel.DataAnnotations;

namespace Simulation_2.Areas.Admin.ViewModels.TeamMembers
{
    public class UpdateTeamMemberVM
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Max 20 symbol!")]
        public string Name { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Max 20 symbol!")]
        public string Job { get; set; }

        public string ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
