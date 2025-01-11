using library_management_api.Data;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDBContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DBConnectionString"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
