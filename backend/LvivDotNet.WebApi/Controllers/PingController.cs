using Microsoft.AspNetCore.Mvc;

namespace LvivDotNet.WebApi.Controllers
{
    /// <summary>
    /// Health check controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : BaseController
    {
        /// <summary>
        /// Ping.
        /// </summary>
        /// <returns> Health check response. </returns>
        [HttpGet]
        public IActionResult Ping()
            => this.Ok();
    }
}