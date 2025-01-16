using Candidate.API.Middleware;
using Candidate.Application.Service;
using Candidate.Application.Validations;
using Candidate.Domain.Interfaces;
using Candidate.Infrastructure.Persistence;
using Candidate.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationsDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("CandidateConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<CandidateProfileValidator>();
builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CandidateProfileValidator>());

builder.Services.AddLogging();

builder.Services.AddScoped<ICandidateService, CandidateService>();  
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply any pending migrations on application startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationsDbContext>();

    try
    {
        context.Database.EnsureCreated();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate(); 
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while creating/updating database: {ex.Message}");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();  // Add this line to enable exception handling globally

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
