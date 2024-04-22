using FingroUssd;
using FingroUssd.Interfaces;
using FingroUssd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add Cache Service
builder.Services.AddScoped<ICacheService, CacheService>();

//Add Menu Service
builder.Services.AddTransient<IMenuService, MenuService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();