using Microsoft.AspNetCore.Http;

namespace ClientRateLimiting.Services
{
  public class HttpClientService
  {
    private readonly IHttpContextAccessor httpContextAccessor;


    public HttpClientService(IHttpContextAccessor httpContextAccessor)
    {
      this.httpContextAccessor = httpContextAccessor;
    }


    public void GetRequest()
    {
      //this.httpContextAccessor.HttpContext.
    }
  }
}
