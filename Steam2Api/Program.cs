using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Steam2Api.Data;
using Steam2Api.Extensions;
using Steam2Api.Services.Game;
using Steam2Api.Services.Invoice;
using Steam2Api.Services.Purchase;
using Steam2Api.Services.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext> (options => 
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();

builder.Services.AddCorsConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("DevelopmentCorsPolicy");

app.MapControllers();


app.Run();
