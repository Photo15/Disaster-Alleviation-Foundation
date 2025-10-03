using DisasterAlleviationFoundation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Create roles
    string[] roles = { "Admin", "Volunteer", "Donor", "RegularUser" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    
    // Create default admin user
    var adminEmail = "admin@disasteralleviation.org";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
    
    // Seed sample volunteer tasks if none exist
    if (!context.VolunteerTasks.Any())
    {
        var sampleTasks = new[]
        {
            new DisasterAlleviationFoundation.Models.VolunteerTask
            {
                Title = "Emergency Food Distribution",
                Description = "Help distribute emergency food supplies to affected families in the evacuation center.",
                TaskDate = DateTime.Now.AddDays(1),
                Location = "Community Center, Main Street",
                RequiredSkills = "Physical Labor",
                EstimatedHours = 4,
                Priority = "High",
                Status = "Open",
                CreatedDate = DateTime.Now
            },
            new DisasterAlleviationFoundation.Models.VolunteerTask
            {
                Title = "Medical Aid Station Support",
                Description = "Assist medical professionals with organizing supplies and helping patients at the aid station.",
                TaskDate = DateTime.Now.AddDays(2),
                Location = "Municipal Health Center",
                RequiredSkills = "First Aid, Communication",
                EstimatedHours = 6,
                Priority = "High",
                Status = "Open",
                CreatedDate = DateTime.Now
            },
            new DisasterAlleviationFoundation.Models.VolunteerTask
            {
                Title = "Cleanup and Debris Removal",
                Description = "Help clear debris and assist with cleanup efforts in affected neighborhoods.",
                TaskDate = DateTime.Now.AddDays(3),
                Location = "Riverside District",
                RequiredSkills = "Physical Labor",
                EstimatedHours = 8,
                Priority = "Medium",
                Status = "Open",
                CreatedDate = DateTime.Now
            },
            new DisasterAlleviationFoundation.Models.VolunteerTask
            {
                Title = "Children's Activity Coordinator",
                Description = "Organize activities and provide emotional support for children in temporary shelters.",
                TaskDate = DateTime.Now.AddDays(4),
                Location = "Temporary Shelter, School Gymnasium",
                RequiredSkills = "Childcare, Psychology",
                EstimatedHours = 5,
                Priority = "Medium",
                Status = "Open",
                CreatedDate = DateTime.Now
            },
            new DisasterAlleviationFoundation.Models.VolunteerTask
            {
                Title = "Supply Inventory Management",
                Description = "Help organize and track incoming donations and supplies.",
                TaskDate = DateTime.Now.AddDays(5),
                Location = "Warehouse District",
                RequiredSkills = "Organization, Computer Skills",
                EstimatedHours = 3,
                Priority = "Low",
                Status = "Open",
                CreatedDate = DateTime.Now
            }
        };
        
        context.VolunteerTasks.AddRange(sampleTasks);
        await context.SaveChangesAsync();
    }
}

app.Run();