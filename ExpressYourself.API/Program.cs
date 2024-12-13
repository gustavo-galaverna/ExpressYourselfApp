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
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();