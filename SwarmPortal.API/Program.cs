using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SwarmPortal.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var authConfig = new AuthConfig();
builder.Configuration.GetSection("Jwt").Bind(authConfig);
var apiConfig = APIConfiguration.Create(builder.Configuration);
builder.Services.AddSingleton<IAuthConfig>(authConfig);

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

builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = authConfig.Issuer,
                    ValidAudience = authConfig.Audience
                };
                o.Authority = authConfig.Authority;
                o.Audience = authConfig.Audience;
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        Console.WriteLine(c.Exception.Message);
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
            });
builder.Services.AddAuthorization(o => {
    // o.AddPolicy("Standard", p => {});
    o.AddPolicy("Admin", p => p.RequireRole("Science"));
});

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStatusGroupCoalescerProvider();
builder.Services.AddLinkGroupCoalescerProvider();

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
