using System.Globalization;

namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
///     Класс Razor шаблона сообщения уведомления по подписке.
/// </summary>
/// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
/// <seealso cref="IRazorTemplate" />
public abstract class RazorTemplate<TDomainObject> : IRazorTemplate
{
    private TextWriter output;

    /// <inheritdoc />
    public abstract string Subject { get; }

    /// <summary>
    ///     Получает предыдущую версию доменного объекта.
    /// </summary>
    /// <value>
    ///     Предыдущая версия доменного объекта.
    /// </value>
    public TDomainObject? Previous { get; set; }

    /// <summary>
    ///     Получает текущую версию доменного объекта.
    /// </summary>
    /// <value>
    ///     Текущая версия доменного объекта.
    /// </value>
    public TDomainObject? Current { get; set; }

    /// <summary>
    /// Контекст системы
    /// </summary>
    public IServiceProvider ServiceProvider { get; set; } = null!;

    protected TextWriter Output
    {
        get { return this.output; }
    }

    /// <inheritdoc />
    public abstract void Execute();

    /// <inheritdoc />
    public void SetWriter(TextWriter writer) => this.output = writer;

    /// <summary>
    ///     Записывает значение.
    /// </summary>
    /// <param name="value">Записываемое значение.</param>
    protected void Write(object value) => this.WriteObject(value);

    /// <summary>
    ///     Записывает значение.
    /// </summary>
    /// <param name="value">Записываемое значение.</param>
    protected void WriteLiteral(object value) => this.WriteObject(value);

    /// <summary>
    /// WriteAttribute is used by Razor runtime v2 and v3.
    /// </summary>
    protected void WriteAttribute(
            string name,
            Tuple<string, int> prefix,
            Tuple<string, int> suffix,
            params object[] fragments) => this.WriteAttributeTo(this.Output, name, prefix, suffix, fragments);

    private void WriteAttributeTo(
            TextWriter writer,
            string name,
            Tuple<string, int> prefix,
            Tuple<string, int> suffix,
            params object[] fragments)
    {
        // For sake of compatibility, this implementation is adapted from
        // System.Web.WebPages.WebPageExecutingBase as found in ASP.NET
        // web stack release 3.2.2:
        // https://github.com/ASP-NET-MVC/aspnetwebstack/releases/tag/v3.2.2
        //
        // Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
        // Licensed under the Apache License, Version 2.0 (the "License")
        // may not use this file except in compliance with the License. You may
        // obtain a copy of the License at
        //
        // http://www.apache.org/licenses/LICENSE-2.0
        //
        // Unless required by applicable law or agreed to in writing, software
        // distributed under the License is distributed on an "AS IS" BASIS,
        // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
        // implied. See the License for the specific language governing permissions
        // and limitations under the License.
        if (fragments.Length == 0)
        {
            this.WriteLiteralTo(writer, prefix.Item1);
            this.WriteLiteralTo(writer, suffix.Item1);
        }
        else
        {
            var first = true;
            var wroteSomething = false;
            foreach (var fragment in fragments)
            {
                var sf = fragment as Tuple<Tuple<string, int>, Tuple<string, int>, bool>;
                var of = sf == null ? (Tuple<Tuple<string, int>, Tuple<object, int>, bool>)fragment : null;

                var ws = sf != null ? sf.Item1.Item1 : of.Item1.Item1;
                var literal = sf != null ? sf.Item3 : of.Item3;
                var val = sf != null ? sf.Item2.Item1 : of.Item2.Item1;

                if (val == null)
                {
                    continue; // nothing to write
                }

                // The special cases here are that the value we're writing might already be a string, or that the
                // value might be a bool. If the value is the bool 'true' we want to write the attribute name instead
                // of the string 'true'. If the value is the bool 'false' we don't want to write anything.
                //
                // Otherwise the value is another object (perhaps an IHtmlString), and we'll ask it to format itself.
                string str;
                var flag = val as bool?;

                switch (flag)
                {
                    case true:
                        str = name;
                        break;
                    case false: continue;
                    default:
                        str = val as string;
                        break;
                }

                if (first)
                {
                    this.WriteLiteralTo(writer, prefix.Item1);
                    first = false;
                }
                else { this.WriteLiteralTo(writer, ws); }

                // The extra branching here is to ensure that we call the Write*To(string) overload when
                // possible.
                if (literal && (str != null))
                {
                    this.WriteLiteralTo(writer, str);
                }
                else if (literal)
                {
                    this.WriteLiteralTo(writer, (string)val);
                }
                else if (str != null)
                {
                    this.WriteTo(writer, str);
                }
                else
                {
                    this.WriteTo(writer, val);
                }

                wroteSomething = true;
            }

            if (wroteSomething)
            {
                this.WriteLiteralTo(writer, suffix.Item1);
            }
        }
    }

    private void WriteObject(object o) => this.Output.Write(o);

    private void WriteLiteralTo(TextWriter writer, string text) => writer.Write(text);

    private void WriteTo(TextWriter writer, object value) => writer.Write(Convert.ToString(value, CultureInfo.InvariantCulture));
}
