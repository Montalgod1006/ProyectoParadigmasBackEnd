using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Steam2Api.Data;
using Steam2Api.Services.Game;
using Steam2Api.Services.Invoice;
using Steam2Api.Services.InvoiceDetail;
using Steam2Api.Services.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext> (options => 
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();
