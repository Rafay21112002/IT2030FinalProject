using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IT2030FinalProject.Data;
using IT2030FinalProject.Models;
using System.Threading.Tasks;
using System.Linq;

namespace IT2030FinalProject.Controllers
{
    public class CharacterController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(AppDbContext context, ILogger<CharacterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Character/All
        public async Task<IActionResult> All()
        {
            var characters = await _context.Characters
                .Include(c => c.Comments)
                .ToListAsync();
            return View(characters);
        }

        // GET: Character/Lelouch
        public async Task<IActionResult> Lelouch()
        {
            var character = await _context.Characters
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.Name == "Lelouch Vi Britannia");

            if (character == null)
            {
                _logger.LogWarning("Lelouch character not found");
                return NotFound();
            }

            return View(character);
        }

        // GET: Character/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Character/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,About,Powers,EyeColor")] Character character)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(character);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Character {character.Name} created successfully");
                    return RedirectToAction(nameof(All));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"Error creating character: {ex.Message}");
                    ModelState.AddModelError("", "Unable to create character. Try again.");
                }
            }
            return View(character);
        }

        // POST: Character/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int characterId, string content, string displayName)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Comment content cannot be empty");
            }

            var comment = new Comment
            {
                CharacterId = characterId,
                Content = content,
                DisplayName = string.IsNullOrEmpty(displayName) ? "Anonymous" : displayName,
                PostedDate = System.DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

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