using Microsoft.EntityFrameworkCore;
using APBD10.Data;
using APBD10.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContext<TripContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();


app.UseAuthorization();
app.MapControllers();

app.Run();