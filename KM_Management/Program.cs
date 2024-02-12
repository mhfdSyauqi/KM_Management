using FluentValidation;
using KM_Management.Commons.Connection;
using KM_Management.Commons.Json;
using Microsoft.AspNetCore.Authentication.Negotiate;
using NetCore.AutoRegisterDi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();


// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = new JsonLowerCaseKeyPolicy();
    });

builder.Services
    .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddCors(options =>
    options.AddPolicy("local", cfg =>
        cfg.WithOrigins("http://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
    )
);

builder.Services.AddRazorPages();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddScoped<ISQLConnectionFactory, SQLConnectionFactory>();
builder.Services.RegisterAssemblyPublicNonGenericClasses()
    .Where(repo => repo.Name.EndsWith("Repository"))
    .AsPublicImplementedInterfaces();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseCors("local");
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
