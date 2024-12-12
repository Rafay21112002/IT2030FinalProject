using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT2030FinalProject.Middleware
{
    public class CharacterAliasMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Dictionary<string, string> _pathMappings;

        public CharacterAliasMiddleware(RequestDelegate next)
        {
            _next = next;
            
            // Initialize path mappings
            _pathMappings = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
            {
                // Character aliases
                { "/Zero", "/Character/Lelouch" },
                { "/Character/Zero", "/Character/Lelouch" },
                
                // Basic routing
                { "/", "/Home/Home" },
                { "/home", "/Home/Home" },
                { "/all", "/Characters/All" },
                
                // Organization aliases
                { "/Organizations/Black-Knights", "/Organizations/Black_Knights" },
                { "/Black-Knights", "/Organizations/Black_Knights" },
                
                // Help routing
                { "/help", "/Characters/Help" },
                
                // Create routing
                { "/Create", "/Characters/Create" }
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path.Value?.TrimEnd('/');

            if (!string.IsNullOrEmpty(requestPath))
            {
                // Check if we have a mapping for this path
                if (_pathMappings.TryGetValue(requestPath, out string? newPath))
                {
                    // Log the path rewrite if needed
                    context.RequestServices
                          .GetService<ILogger<CharacterAliasMiddleware>>()
                          ?.LogInformation($"Rewriting path from {requestPath} to {newPath}");

                    // Update the request path
                    context.Request.Path = new PathString(newPath);
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    // Extension method to make it easier to add the middleware
    public static class CharacterAliasMiddlewareExtensions
    {
        public static IApplicationBuilder UseCharacterAlias(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CharacterAliasMiddleware>();
        }
    }
}