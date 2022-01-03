using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Persistence;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
AddSwagger(builder.Services);

Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

void AddSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "GloboTicket Ticket Management API",
        });


    });
}

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboTicket Management API");
});
app.UseCors("Open");

app.MapControllers();

app.Run();
