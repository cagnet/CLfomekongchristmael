using CustomerOrders.Api.Database;
using CustomerOrders.Api.Database.Repositories;
using CustomerOrders.Api.Middlewares;
using CustomerOrders.Business.Repositories;
using CustomerOrders.Business.Services;
using CustomerOrders.Data.InMemory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Customer Orders API", Version = "v1" });
});
builder.Services.AddSingleton<InMemoryDatabase>();
builder.Services.AddDbContext<CustomerOrdersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddScoped<ICustomerRepository, DurableCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, DurableOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Orders API v1");
    options.RoutePrefix = "swagger";
});
app.MapControllers();
app.Run();
public partial class Program;
