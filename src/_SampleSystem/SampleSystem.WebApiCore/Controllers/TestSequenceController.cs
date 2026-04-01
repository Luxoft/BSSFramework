using Framework.Configuration.BLL;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestSequenceController(ISequenceBLLFactory sequenceBllFactory) : ControllerBase
{
    [HttpPost]
    public long Check()
    {
        var bll = sequenceBllFactory.Create();

        var nextNumber = bll.GetNextNumber("Test");

        Thread.Sleep(5000);

        return nextNumber;
    }
}
