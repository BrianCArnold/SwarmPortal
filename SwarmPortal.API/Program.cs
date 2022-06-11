using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStatusGroupCoalescerProvider();
builder.Services.AddLinkGroupCoalescerProvider();
builder.Services.AddStaticStatusProvider();
builder.Services.AddStaticLinkProvider();
// builder.Services.Configure<JsonOptions>(options =>
// {
//     options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
//     options.JsonSerializerOptions.IncludeFields = true;
//     options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
//     options.JsonSerializerOptions.PropertyNamingPolicy = null;
//     options.JsonSerializerOptions.WriteIndented = true;
// });



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
