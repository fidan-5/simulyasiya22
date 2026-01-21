using Microsoft.AspNetCore.Mvc;
using Simulation_2.Areas.Admin.ViewModels.TeamMembers;
using Simulation_2.DAL;
using Simulation_2.Models;
using Simulation_2.Utilities.Extensions;
using Simulation_2.Utilities.Enums;

namespace Simulation_2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMembersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TeamMembersController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<TeamMembers> members = _context.TeamMembers.ToList();
            return View(members);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamMemberVM createTeamMemberVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!createTeamMemberVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "File type must be an image!");
                return View();
            }
            if (createTeamMemberVM.Photo.ValidateSize(2, FileSize.MB))
            {
                ModelState.AddModelError("Photo", "Maximum file size is 2 MB!");
                return View();
            }
            TeamMembers member = new TeamMembers()
            {
                Name = createTeamMemberVM.Name,
                Job = createTeamMemberVM.Job,
                ImageUrl = await createTeamMemberVM.Photo.CreateFile(_env.WebRootPath, "img")
            };
            await _context.AddRangeAsync(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if(id==null || id < 1)
            {
                return BadRequest();
            }
            TeamMembers members = _context.TeamMembers.FirstOrDefault(m=>m.Id == id);
            if (members == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(members);
            }
            UpdateTeamMemberVM vm = new UpdateTeamMemberVM()
            {
                Name = members.Name,
                Job = members.Job,
                ImageUrl = members.ImageUrl
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateTeamMemberVM updateTeamMemberVM)
        {
            if (!ModelState.IsValid)
            {
                return View(updateTeamMemberVM);
            }
            TeamMembers members = _context.TeamMembers.FirstOrDefault(m=>m.Id==id);
            if(updateTeamMemberVM.Photo is not  null)
            {
                if (!updateTeamMemberVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be an image!");
                    return View();
                }
                if (updateTeamMemberVM.Photo.ValidateSize(2, FileSize.MB))
                {
                    ModelState.AddModelError("Photo", "Maximum file size is 2 MB!");
                    return View();
                }
                string fileName = await updateTeamMemberVM.Photo.CreateFile(_env.WebRootPath, "img");
                members.ImageUrl = fileName;
            }
            members.Name = updateTeamMemberVM.Name;
            members.Job = updateTeamMemberVM.Job;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int? id)
        {
            TeamMembers members = _context.TeamMembers.FirstOrDefault(members=>members.Id==id);
            return View(members);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }
            TeamMembers members = _context.TeamMembers.FirstOrDefault(m => m.Id == id);
            if (members == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(members);
            }
            members.ImageUrl.DeleteFile(_env.WebRootPath,"img");
            _context.TeamMembers.Remove(members);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
