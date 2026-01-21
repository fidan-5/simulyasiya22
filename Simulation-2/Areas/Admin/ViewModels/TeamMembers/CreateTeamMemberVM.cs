using System.ComponentModel.DataAnnotations;

namespace Simulation_2.Areas.Admin.ViewModels.TeamMembers
{
    public class CreateTeamMemberVM
    {
        [Required]
        [MaxLength(20,ErrorMessage ="Max 20 symbol!")]
        public string Name { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Max 20 symbol!")]
        public string Job { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
