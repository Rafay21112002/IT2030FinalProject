using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging; 
using IT2030FinalProject.Models; 
using IT2030FinalProject.Data; 
using IT2030FinalProject.Middleware; 
 
var builder = WebApplication.CreateBuilder(args); 
 
// Add services to the container. 
builder.Services.AddControllersWithViews(); 
 
// Configure database 
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 
 
// Configure logging 
builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug(); 
builder.Logging.AddEventSourceLogger(); 
 
// Add memory cache 
builder.Services.AddMemoryCache(); 
builder.Services.AddDistributedMemoryCache(); 
 
var app = builder.Build(); 
 
// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment()) 
{ 
    app.UseDeveloperExceptionPage(); 
} 
else 
{ 
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts(); 
} 
 
app.UseHttpsRedirection(); 
app.UseStaticFiles(); 
app.UseRouting(); 
 
// Add custom middleware for character aliases 
app.UseMiddleware<CharacterAliasMiddleware>(); 
 
app.UseAuthorization(); 
 
app.MapControllerRoute( 
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}"); 
 
app.Run(); 
