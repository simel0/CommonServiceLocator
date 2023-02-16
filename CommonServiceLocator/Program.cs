using CommonServiceLocator.HostBuilders;
using CommonServiceLocator.Services;
using ServiceLocation;

var builder = WebApplication.CreateBuilder(args);

//Host.CreateDefaultBuilder(args)
//    .ConfigureCustomBuilder()
//    .ConfigureWebHostDefaults(webBuilder =>
//    {
//        webBuilder.UseStartup<Startup>();
//    });
builder.Host.ConfigureCustomBuilder(); //longnt: ServiceLocator

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IService1, Service1>();
var y = ServiceLocator.Current.GetService<IService1>();
builder.Services.AddServiceLocator(); //longnt: ServiceLocator

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//using (IServiceScope scope = app.Services.CreateScope())
using (var scope = app.Services.CreateServiceLocatorScope()) // use this for hosted service or anywhere need a scope
{
    var x = ServiceLocator.Current.GetService<IService1>();
}

app.MapControllers();

app.Run();