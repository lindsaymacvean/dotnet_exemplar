using dotnet_exemplar.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

// Add Swagger UI static JS for auto api-key, prefill dev key if running locally.
app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
    settings.AdditionalSettings["persistAuthorization"] = true;
    settings.AdditionalSettings["injectScripts"] = new[] { "/swagger-autokey.js" };
    // Setting variable the JS can read to know about dev
    if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
    {
        settings.AdditionalSettings["DevApiKey"] = "f6e4d7fe-8e76-4d8c-bf76-23c7fad51eab";
    }
});

// Unnecessary because Angular is used for the frontend
// app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.Run();

public partial class Program { }
