using DiagnosticContextLib;
using Prometheus;
using WebAPI.Jobs;
using WebAPI.Repositories;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IServiceOne, ServiceOne>();
builder.Services.AddTransient<IServiceTwo, ServiceTwo>();
builder.Services.AddTransient<IRepositoryOne, RepositoryOne>();
builder.Services.AddTransient<IRepositoryTwo, RepositoryTwo>();
builder.Services.AddScoped<IDiagnosticContext, DiagnosticContext>();

builder.Services.AddHostedService<OneBackgroundJob>();
builder.Services.AddHostedService<TwoBackgroundJob>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpMetrics();
app.MapMetrics();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<DiagnosticContextMiddleware>();
app.Run();