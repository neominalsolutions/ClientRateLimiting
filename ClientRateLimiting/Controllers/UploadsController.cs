using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TaskAssignmentAppNTier.Attributes;

namespace ClientRateLimiting.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UploadsController : ControllerBase
  {

    // upload IFormFile
    // download base64 string kullanalım.

    [HttpPost]
    // [FileLimit(15)]
    public async Task<IActionResult> Upload([FromForm] IEnumerable<IFormFile> files)
    {

      foreach (var formFile in files)
      {
        string extension = System.IO.Path.GetExtension(formFile.FileName);

        if (formFile.Length > 0)
        {
          var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", formFile.FileName);

          using (var stream = System.IO.File.Create(filePath))
          {
            await formFile.CopyToAsync(stream); // dosyay kopyalama işlemlerini async ve stream olarak yapın.
          }
        }
      }

      return Ok();
    }

  }
}
