using Microsoft.EntityFrameworkCore;
using Medical_Appointment_System.Contexts;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;
using OnlineMedicalAppointmentSystem.Repositories;
using OnlineMedicalAppointmentSystem.Services.Interfaces;
using OnlineMedicalAppointmentSystem.Services;

var builder = WebApplication.CreateBuilder(args);
//Database Connection and Dependency Injection
builder.Services.AddDbContext<MedicalAppointmentSystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAvailabilitySlotRepository, AvailabilitySlotRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IServiceServices, ServiceServices>();
builder.Services.AddScoped<IAvailabilitySlotService, AvailabilitySlotService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
