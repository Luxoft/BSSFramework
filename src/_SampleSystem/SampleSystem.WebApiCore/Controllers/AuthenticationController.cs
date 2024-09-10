using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpGet]
    public bool IsAuthenticated()
    {
        return this.HttpContext.User.Identity!.IsAuthenticated;
    }
}
