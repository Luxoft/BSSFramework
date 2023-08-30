using Framework.Configuration.BLL;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[Action]")]
[ApiController]
public class TestSequenceController : ControllerBase
{
    private readonly ISequenceBLLFactory sequenceBllFactory;

    public TestSequenceController(ISequenceBLLFactory sequenceBllFactory) => this.sequenceBllFactory = sequenceBllFactory;

    [HttpPost]
    public long Check()
    {
        var bll = this.sequenceBllFactory.Create();

        var nextNumber = bll.GetNextNumber("Test");

        Thread.Sleep(5000);

        return nextNumber;
    }
}
