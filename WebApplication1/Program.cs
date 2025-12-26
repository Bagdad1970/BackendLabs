using System.Text.Json;
using Dapper;
using FluentValidation;
using WebApplication1.Jobs;
using WebApplication1.BLL.Services;
using WebApplication1.Config;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DAL.Repositories;
using WebApplication1.Validators;
using IOrderRepository = WebApplication1.DAL.Interfaces.IOrderRepository;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddScoped<UnitOfWork>();

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));


builder.Services.AddScoped<IAuditLogOrderRepository, AuditLogOrderRepository>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.AddScoped<RabbitMqService>();


builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddScoped<ValidatorFactory>();

builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddSwaggerGen();


builder.Services.AddHostedService<OrderGenerator>();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

Migrations.Program.Main([]); 

app.Run();