using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace csharp_webhook_based_jobscheduler.API.Features.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Content(object obj)
        {
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }
    }
}
