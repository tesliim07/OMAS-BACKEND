using Microsoft.EntityFrameworkCore;
using Medical_Appointment_System.Contexts;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;
using OnlineMedicalAppointmentSystem.Repositories;
using OnlineMedicalAppointmentSystem.Services.Interfaces;
using OnlineMedicalAppointmentSystem.Services;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
var pooledConn = builder.Configuration.GetConnectionString("PooledConnection");

Console.WriteLine("=== CONNECTION STRING DEBUG ===");
Console.WriteLine($"DefaultConnection: {defaultConn}");
Console.WriteLine($"PooledConnection: {pooledConn}");
Console.WriteLine("================================");

//Hangfire configuration
builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(
    options =>
    {

        options.UseNpgsqlConnection(pooledConn);
    }
    ));
builder.Services.AddHangfireServer();
//Database Connection and Dependency Injection
builder.Services.AddDbContext<MedicalAppointmentSystemDbContext>(options =>
    options.UseNpgsql(pooledConn));
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAvailabilitySlotRepository, AvailabilitySlotRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IServiceServices, ServiceServices>();
builder.Services.AddScoped<IAvailabilitySlotService, AvailabilitySlotService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://omas-frontend-b7fyf.ondigitalocean.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("dev");

// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

// Register recurring job in a background task to avoid startup lock issues
Task.Run(async () =>
{
    await Task.Delay(5000); // Wait 5 seconds for app to fully start
    RecurringJob.AddOrUpdate<IAppointmentService>(
        "find_email_addresses",
        s => s.SendAppointmentReminder(3),
        Cron.Daily(7)
    );
});

//Create recurring job (once app starts)
//RecurringJob.AddOrUpdate<IAppointmentService>(
//    "find_email_addresses",                    // Job ID
//    s => s.SendAppointmentReminder(3),              // Method to run
//    Cron.Daily(7));                                   // Schedule: every day

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
