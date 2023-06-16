
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskAssignmentAppNTier.Attributes
{
  /// <summary>
  /// AOP Programing sağlar.
  /// </summary>
  public class FileLimitAttribute : Attribute, IAsyncActionFilter
  {
    /// <summary>
    /// Web uygulamasındaki HttpContext class üzerinden erişmemizi sağlayan bir interface
    /// </summary>
    private long maxFileSize;



    public FileLimitAttribute(long maxFileSize)
    {

      this.maxFileSize = maxFileSize;
    }

    /// <summary>
    /// Async tercih sebebi middleware gibi çalışsınm aynı zamanda, ayrı bir task açsın uygulamanın yük bindirmesin diye tercih ettik.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

      if (context.HttpContext.Request.Method == HttpMethods.Post)
      {
        if (context.HttpContext.Request.Form.Files.Count > 0)
        {

          long size = context.HttpContext.Request.Form.Files.Sum(f => f.Length);

          if (size > maxFileSize)
          {
            context.Result = new BadRequestResult();
            await Task.CompletedTask;
          }
          else
          {
            await next();
          }

        }

      }
    }
  }   
}
