using System.Globalization;

using CommonFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.Metadata;

public abstract partial class RazorTemplate<TDomainObject> : IMessageTemplate<TDomainObject>
{
    private RenderingState? state;

    protected RenderingState State => this.state ?? throw new InvalidOperationException("State not initialized");

    /// <summary>
    ///     Получает предыдущую версию доменного объекта.
    /// </summary>
    /// <value>
    ///     Предыдущая версия доменного объекта.
    /// </value>
    protected TDomainObject? Previous => this.State.Versions.Previous;

    /// <summary>
    ///     Получает текущую версию доменного объекта.
    /// </summary>
    /// <value>
    ///     Текущая версия доменного объекта.
    /// </value>
    protected TDomainObject? Current => this.State.Versions.Current;

    /// <summary>
    /// Контекст системы
    /// </summary>
    protected IServiceProvider ServiceProvider => this.State.ServiceProvider;

    protected CultureInfo? Culture => this.ServiceProvider.GetService<ICultureSource>()?.Culture;

    protected TextWriter Writer => this.State.Writer;

    public abstract string Subject { get; }


    public (string Subject, string Body) Render(IServiceProvider serviceProvider, IObjectsVersion<TDomainObject> versions)
    {
        this.state = new RenderingState(new StringWriter(), serviceProvider, versions);

        this.Execute();

        return (this.Subject, this.State.Writer.ToString());
    }

    protected record RenderingState(StringWriter Writer, IServiceProvider ServiceProvider, IObjectsVersion<TDomainObject> Versions);
}
