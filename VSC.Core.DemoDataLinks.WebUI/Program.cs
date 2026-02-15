using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VSC.Core.Data;
using VSC.Core.DemoContacts.Services;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Identity.Data;
using VSC.Core.Identity.DataModel;
using VSC.Core.Identity.Services;
using VSC.Core.Services;
using VSC.Core.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database connections

var identityConnectionString =
builder.Configuration.GetConnectionString("IdentityDatabaseConnection");
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(identityConnectionString)
    .UseLoggerFactory(new NullLoggerFactory()));

var sessionDataConnectionString = builder.Configuration.GetConnectionString("SessionDatabaseConnection");
builder.Services.AddDbContext<SessionStateDbContext>(options => options.UseSqlServer(sessionDataConnectionString)
    .UseLoggerFactory(new NullLoggerFactory()));

var demoDataConnectionString = builder.Configuration.GetConnectionString("DemoDatabaseConnection");
builder.Services.AddDbContext<DemoDatabaseContext>(options => options.UseSqlServer(demoDataConnectionString)
    .UseLoggerFactory(new NullLoggerFactory()));

//Session Core
builder.Services.AddTransient<ISessionStateStore, SessionStateStore>();

// Application Services
builder.Services.AddTransient<IInstanceService<ContactType>, ContactTypeService>();
builder.Services.AddTransient<IInstanceService<ContactData>, ContactDataService>();
builder.Services.AddTransient<IInstanceService<ContactGroup>, ContactGroupService>();
builder.Services.AddTransient<IInstanceService<Person>, PeopleDataService>();
builder.Services.AddTransient<IInstanceService<Interest>, InterestService>();
builder.Services.AddTransient<ILinkService<PersonalInterest>, PersonalInterestService>();


//Identity
builder.Services.AddTransient<IUserStore<UserCredential>, UserStore>();
builder.Services.AddTransient<IRoleStore<AssignedUserRole>, RoleStore>();
builder.Services.AddIdentity<UserCredential, AssignedUserRole>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opts => {
    opts.Password.RequiredLength = 8;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireDigit = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/SignIn";
    options.Cookie.Name = "VSC.Core.Demo.ContactLinks";
});

// To ensure custom claims are added to new identity when the principal is refreshed.
builder.Services.ConfigureOptions<ConfigureSecurityStampOptions>();

//Authorisation Policy: all controllers and actions require an authenticated user (except [AllowAnonymous]). 
var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser().Build();
builder.Services.AddMvcCore(options =>
{
    options.Filters.Add(new AuthorizeFilter(policy));
    options.EnableEndpointRouting = false;
}).AddRazorViewEngine();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
