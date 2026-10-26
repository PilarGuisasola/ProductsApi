using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Repositories.Interfaces;
using ProductsApi.Repositories.Implementations;
using ProductsApi.Services.Interfaces;
using ProductsApi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProductsApiContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString(
            "ProductsApiDBConnectionString")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();