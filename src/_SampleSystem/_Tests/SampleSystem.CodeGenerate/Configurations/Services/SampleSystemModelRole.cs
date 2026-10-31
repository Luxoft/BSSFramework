using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Models;

namespace SampleSystem.CodeGenerate.Configurations.Services;

/// <summary>
/// Custom roles of SampleSystem models
/// </summary>
public static class SampleSystemModelRole
{
    /// <summary>
    /// Role of models for generating collection changes
    /// </summary>
    public static readonly ModelRole ComplexChange = new(nameof(ComplexChange), DirectMode.In);
}
