using GPOS.Core.Helper;
using GPOS.Infrastructure.Context;
using GPOS.Infrastructure.UOW;
using GPOS.MarketPlaceApi.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var gposConnectionString = builder.Configuration.GetConnectionString("GPOSConnection");
var gposConnectionStringBuilder = new SqlConnectionStringBuilder(gposConnectionString);
gposConnectionStringBuilder.Password = Crypto.DecryptStringAES(gposConnectionStringBuilder.Password, SecurityHelper.SecForEncryption);
gposConnectionString = gposConnectionStringBuilder.ToString();
builder.Services.AddDbContext<GPOSDbContext>(o =>
{
    o.UseSqlServer(gposConnectionString);
});
List<ApiKeyModel> apiKeys = new List<ApiKeyModel>();
try
{
    apiKeys = builder.Configuration.GetSection("MarketPlaceApiKey").Get<List<ApiKeyModel>>();
}
catch (Exception ex)
{
    throw new Exception(ex.Message);
}
builder.Services.AddSingleton(apiKeys);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();




