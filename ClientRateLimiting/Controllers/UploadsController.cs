using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    [FileLimit(15)]
    public IActionResult Upload([FromForm] IEnumerable<IFormFile> files)
    {
      return Ok();
    }

  }
}
