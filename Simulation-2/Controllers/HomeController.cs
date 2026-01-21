using Microsoft.AspNetCore.Mvc;
using Simulation_2.DAL;
using Simulation_2.Models;

namespace Simulation_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<TeamMembers> members = _context.TeamMembers.ToList();

            return View(members);
        }
    }
}
