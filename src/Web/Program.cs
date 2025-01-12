const string Allow_Origin_Policy = "Allow-Origin-Policy";

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(Allow_Origin_Policy, builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseCors(Allow_Origin_Policy);
// Configure routing
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}");

// Optionally add an API-specific route
app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller}/{action}");

app.MapControllers();

//app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

//app.MapEndpoints();

app.Run();

public partial class Program { }
