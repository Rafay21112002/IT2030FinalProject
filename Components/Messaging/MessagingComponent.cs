using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using IT2030FinalProject.Data;
using IT2030FinalProject.Models;

namespace IT2030FinalProject.Components
{
    public class MessagingViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public MessagingViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string entityType, int entityId)
        {
            var comments = await GetCommentsAsync(entityType, entityId);
            var viewModel = new MessagingViewModel
            {
                EntityType = entityType,
                EntityId = entityId,
                Comments = comments,
                NewCommentContent = string.Empty,
                DisplayName = string.Empty
            };

            return View(viewModel);
        }

        private async Task<List<Comment>> GetCommentsAsync(string entityType, int entityId)
        {
            return entityType.ToLower() switch
            {
                "character" => await _context.Comments
                    .Where(c => c.CharacterId == entityId)
                    .OrderByDescending(c => c.PostedDate)
                    .ToListAsync(),

                "organization" => await _context.Comments
                    .Where(c => c.OrganizationId == entityId)
                    .OrderByDescending(c => c.PostedDate)
                    .ToListAsync(),

                _ => new List<Comment>()
            };
        }
    }

    public class MessagingViewModel
    {
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public string NewCommentContent { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public static class MessagingExtensions
    {
        public static string GetEntityName(this MessagingViewModel model)
        {
            return model.EntityType.ToLower() switch
            {
                "character" => "Character",
                "organization" => "Organization",
                _ => "Unknown"
            };
        }
    }
}