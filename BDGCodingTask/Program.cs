using BDGCodingTask.Services;
using BDGCodingTask.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IExchangeDataLoaderService,ExchangeDataLoaderService>();
builder.Services.AddScoped<IUserInstructionsService, UserInstructionsService>();

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
