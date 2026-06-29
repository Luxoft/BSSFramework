using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("configApi/[controller]/[action]")]
public class ConfigMainController : Framework.Configuration.WebApi.ConfigMainController;
