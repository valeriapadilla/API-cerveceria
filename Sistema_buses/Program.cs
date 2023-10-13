using Sistema_buses.DbContexts;
using Sistema_buses.Interfaces;
using Sistema_buses.Repository;
using Sistema_buses.Services;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

//El DBContext a utilizar
builder.Services.AddSingleton<PgsqlDbContext>();

//Los repositorios
builder.Services.AddScoped<ICargadorRepository, CargadorRepository>();

//Aqui agregamos los servicios asociados para cada EndPoint
builder.Services.AddScoped<CargadorServices>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
