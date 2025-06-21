using Identity.API.Routes;
using Identity.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add app services
builder.Services.AddSingleton<IIdentifyService, IdentifyService>();

// Add services to the container.
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

app.AddRoutes();
app.UseHttpsRedirection();

app.Run();
