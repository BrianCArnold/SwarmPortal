using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

const string debugCorsOriginName = "DebugCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: debugCorsOriginName,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200");
                      });
});

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStatusGroupCoalescerProvider();
builder.Services.AddLinkGroupCoalescerProvider();

var apiConfig = APIConfiguration.Create(builder.Configuration);
builder.Services.AddAPIConfiguration();
builder.Services.AddDockerConfiguration();
builder.Services.AddStaticFileConfiguration();
builder.Services.AddSQLiteFileConfiguration();
builder.Services.AddSQLiteContext();


if (apiConfig.EnableStaticFileLinks)
builder.Services.AddStaticLinkFileProvider();
if (apiConfig.EnableDockerNodeStatus)
builder.Services.AddDockerNodeStatusProvider();
if (apiConfig.EnableDockerServiceStatus)
builder.Services.AddDockerServiceStatusProvider();
if (apiConfig.EnableDockerServiceLinks)
builder.Services.AddDockerServiceLinkProvider();
if (apiConfig.EnableSQLiteLinks)
builder.Services.AddSQLiteLinkProvider();

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

#if !DEBUG
app.UseHttpsRedirection();
#endif

app.UseCors(debugCorsOriginName);

app.UseAuthorization();

app.MapControllers();

app.Run();
