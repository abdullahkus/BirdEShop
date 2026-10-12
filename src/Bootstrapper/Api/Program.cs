
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

var catalogAssembly = typeof(Catalog.CatalogModule).Assembly;
var basketAssembly = typeof(Basket.BasketModule).Assembly;
var orderingAssembly = typeof(Ordering.OrderingModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly, orderingAssembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services
    .AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();