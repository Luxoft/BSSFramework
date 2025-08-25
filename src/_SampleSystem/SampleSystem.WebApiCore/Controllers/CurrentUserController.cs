using SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class CurrentUserController(ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public string GetCurrentUserName()
    {
        return currentUser.Name;
    }
}
