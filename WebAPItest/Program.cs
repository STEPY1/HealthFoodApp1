using MongoDB.Driver;
using WebAPItest.Services;

var builder = WebApplication.CreateBuilder(args);

string mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
string dbName = builder.Configuration["DatabaseSettings:DatabaseName"];

var settings = MongoClientSettings.FromConnectionString(mongoConnectionString);
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var mongoClient = new MongoClient(settings);

builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<FoodItemService>();
builder.Services.AddSingleton<RestaurantService>();
builder.Services.AddSingleton(mongoClient.GetDatabase(dbName));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
