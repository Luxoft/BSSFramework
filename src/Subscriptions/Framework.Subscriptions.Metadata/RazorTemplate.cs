using System.Globalization;

using Anch.Core;

using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.Metadata;

public abstract partial class RazorTemplate<TRenderingObject> : IMessageTemplate<TRenderingObject>
    where TRenderingObject : class
{
    private RenderingState? state;

    protected RenderingState State => this.state ?? throw new InvalidOperationException("State not initialized");

    /// <summary>
    ///     Gets the previous version of the domain object.
    /// </summary>
    /// <value>
    ///     Previous version of the domain object.
    /// </value>
    protected TRenderingObject? Previous => this.State.Versions.Previous;

    /// <summary>
    ///     Gets the current version of the domain object.
    /// </summary>
    /// <value>
    ///     Current version of the domain object.
    /// </value>
    protected TRenderingObject? Current => this.State.Versions.Current;

    /// <summary>
    /// System context
    /// </summary>
    protected IServiceProvider ServiceProvider => this.State.ServiceProvider;

    protected CultureInfo? Culture => this.ServiceProvider.GetService<ICultureSource>()?.Culture;

    protected TextWriter Writer => this.State.Writer;

    public abstract string Subject { get; }


    public ValueTask<(string Subject, string Body)> Render(
        IServiceProvider serviceProvider,
        DomainObjectVersions<TRenderingObject> versions,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        this.state = new RenderingState(new StringWriter(), serviceProvider, versions);

        this.Execute();

        return new((this.Subject, this.State.Writer.ToString()));
    }

    protected record RenderingState(StringWriter Writer, IServiceProvider ServiceProvider, DomainObjectVersions<TRenderingObject> Versions);
}
