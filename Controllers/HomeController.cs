using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IT2030FinalProject.Data;
using IT2030FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IT2030FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get summary counts for the home page
            var viewModel = new HomeViewModel
            {
                CharacterCount = await _context.Characters.CountAsync(),
                OrganizationCount = await _context.Organizations.CountAsync(),
                RecentComments = await _context.Comments
                    .OrderByDescending(c => c.PostedDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An error occurred in the application.");
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }

    public class HomeViewModel
    {
        public int CharacterCount { get; set; }
        public int OrganizationCount { get; set; }
        public List<Comment> RecentComments { get; set; } = new List<Comment>();
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; } = string.Empty;
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}