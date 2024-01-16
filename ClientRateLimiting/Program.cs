using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(); // deðiþmedi.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// IConfiguration interfacelerinden gelenler builder.Configuration

builder.Services.AddOptions();
builder.Services.AddMemoryCache(); // InMemory Cache Service
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting")); // deðiþti.
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

builder.Services.AddHttpContextAccessor(); // IHttpContextAccessor ile HttpContext baðlantýsýný service class'ýmýzda eriþmek için eklendik.

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
// son verisyon için ekledik
// en güncel clientrate paketi için bu ayarý eklememiz lazým.
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// ConfigureServices içindeki yukarýdaki builder.Services olarak yazdýk.
var app = builder.Build();

// IWebHostEnvironment env yerine app.Environment

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();  // eklenecek
  app.UseSwaggerUI(); // eklenecek
  app.UseDeveloperExceptionPage();
}

app.UseClientRateLimiting(); // client rate limiting
app.UseHttpsRedirection();

//app.UseRouting(); // kaldýrýlacak

app.UseStaticFiles();

app.UseAuthorization();


app.MapControllers(); // eklenecek


//app.Use(async (context, next) =>
//{

//  if(context.Request.Method == HttpMethods.Post)
//  {
//    if(context.Request.Form.Files.Count > 0)
//    {

//      long size = context.Request.Form.Files.Sum(f => f.Length);

//      if(size > 500000)
//      {

//      }
//      else
//      {
//        await next();
//      }

//    }

// await next();
//  }
//});

app.Run(); // eklenecek

//app.UseEndpoints(endpoints => // kaldýrýlacak
//{
//  endpoints.MapControllers();
//});

