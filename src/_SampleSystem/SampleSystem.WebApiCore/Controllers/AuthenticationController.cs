using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpGet(nameof(IsAuthenticated))]
    public bool IsAuthenticated()
    {
        return this.HttpContext.User.Identity!.IsAuthenticated;
    }
}
