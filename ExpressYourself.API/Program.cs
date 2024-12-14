using ExpressYourself.API.BuilderExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddContext();
builder.AddDependencies();
builder.AddHttpClient();
builder.AddMappings();
builder.AddSwagger();
builder.AddBackgroundServices();
builder.AddRedis();
builder.Services.AddControllers();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();
app.MapControllers();

app.Run();