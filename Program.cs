using asp_vpn_checker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ILogger<VPNChecker> logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<VPNChecker>>();
builder.Services.AddSingleton<IVPNChecker, VPNChecker>((x) => new VPNChecker(logger));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
