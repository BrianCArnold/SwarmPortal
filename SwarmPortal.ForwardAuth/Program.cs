using SwarmPortal.ForwardAuth;

var builder = WebApplication.CreateBuilder(args);


{
    var forwardAuthConfig = new ForwardAuthConfig();
    builder.Configuration.Bind("ForwardAuthConfig", forwardAuthConfig);

    builder.Services.AddSingleton(forwardAuthConfig);
}
{
    var clientDictionary = new OAuthClientRegistry();
    builder.Configuration.Bind("OAuthClients", clientDictionary);
    builder.Services.AddSingleton(clientDictionary);
}
// Add services to the container.

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
