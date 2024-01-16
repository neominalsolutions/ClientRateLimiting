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


builder.Services.AddControllers(); // de�i�medi.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// IConfiguration interfacelerinden gelenler builder.Configuration

builder.Services.AddOptions();
builder.Services.AddMemoryCache(); // InMemory Cache Service
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting")); // de�i�ti.
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

builder.Services.AddHttpContextAccessor(); // IHttpContextAccessor ile HttpContext ba�lant�s�n� service class'�m�zda eri�mek i�in eklendik.

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
// son verisyon i�in ekledik
// en g�ncel clientrate paketi i�in bu ayar� eklememiz laz�m.
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// ConfigureServices i�indeki yukar�daki builder.Services olarak yazd�k.
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

//app.UseRouting(); // kald�r�lacak

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

//app.UseEndpoints(endpoints => // kald�r�lacak
//{
//  endpoints.MapControllers();
//});

