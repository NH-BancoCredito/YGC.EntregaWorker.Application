using Steeltoe.Extensions.Configuration.ConfigServer;
using Entrega.Api.Middleware;
using EntregaWorker.Application;
using EntregaWorker.Infrastructure;
using Entrega.Worker.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddConfigServer(
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    })
    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Capa de aplicacion
builder.Services.AddApplication();

//Capa de infra
//var connectionString = builder.Configuration.GetConnectionString("dbEntregas-cnx");
var connectionString = builder.Configuration["dbEntregas-cnx"];
builder.Services.AddInfraestructure(builder.Configuration);
//builder.Services.AddInfraestructure(connectionString);
//Adiconando el background service
builder.Services.AddHostedService<ActualizarStocksWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Adicionar middleware customizado para tratar las excepciones
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();
