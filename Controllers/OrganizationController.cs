using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IT2030FinalProject.Data;
using IT2030FinalProject.Models;
using System.Threading.Tasks;
using System.Linq;

namespace IT2030FinalProject.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(AppDbContext context, ILogger<OrganizationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Organization/All
        public async Task<IActionResult> All()
        {
            var organizations = await _context.Organizations
                .Include(o => o.Leader)
                .Include(o => o.Comments)
                .ToListAsync();
            return View(organizations);
        }

        // GET: Organization/BlackKnights
        public async Task<IActionResult> BlackKnights()
        {
            var organization = await _context.Organizations
                .Include(o => o.Leader)
                .Include(o => o.Comments)
                .FirstOrDefaultAsync(o => o.Name == "Black Knights");

            if (organization == null)
            {
                _logger.LogWarning("Black Knights organization not found");
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organization/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int organizationId, string content, string displayName)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Comment content cannot be empty");
            }

            var comment = new Comment
            {
                OrganizationId = organizationId,
                Content = content,
                DisplayName = string.IsNullOrEmpty(displayName) ? "Anonymous" : displayName,
                PostedDate = System.DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Redirect back to the organization page
            return RedirectToAction(nameof(All));
        }

        // Error handling
        private IActionResult HandleError(string message)
        {
            _logger.LogError(message);
            return View("Error", new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}