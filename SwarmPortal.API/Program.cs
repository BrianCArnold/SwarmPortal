using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SwarmPortal.API;
using SwarmPortal.Common;

var builder = WebApplication.CreateBuilder(args);
Directory.CreateDirectory("persist");
if (!File.Exists("persist/settings.json"))
{
    File.Copy("ExampleFiles/settings.json", "persist/settings.json");
}
builder.Configuration.AddJsonFile("persist/settings.json", false);

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
                          policy.WithOrigins(builder.Configuration["CORSUrl"])
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

// Add services to the container.

builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = authConfig.RequireHttps;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
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

                        // c.Response.StatusCode = 500;
                        // c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
            });
builder.Services.AddAuthorization();

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddStatusGroupDictionaryProvider()
    .AddLinkGroupDictionaryProvider()
    .AddStatusCoalescedItemProvider()
    .AddLinkCoalescedItemProvider()
    .AddStatusOrderingProvider()
    .AddLinkOrderingProvider()
    .AddStatusRoleFilteringProvider()
    .AddLinkRoleFilteringProvider()
    .AddAPIConfiguration()
    .AddDockerConfiguration()
    .AddStaticFileConfiguration()
    .AddSQLiteAccessors()
    .AddSwarmPortalContext(builder.Configuration);


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

//Intended to be hosted behind a reverse proxy in a docker container.
// #if !DEBUG
// app.UseHttpsRedirection();
// #endif

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(debugCorsOriginName);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();

