using Microsoft.AspNetCore.Mvc;

namespace LvivDotNet
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets a value indicating whether the user has been authenticated.
        /// </summary>
        public bool IsAuthenticated => this?.User?.Identity?.IsAuthenticated ?? false;
    }
}
