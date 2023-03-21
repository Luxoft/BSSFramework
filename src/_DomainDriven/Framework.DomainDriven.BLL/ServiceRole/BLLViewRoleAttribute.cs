using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Transfering;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Атрибут указания генерации фасадного и BLL слоёв
/// </summary>
public class BLLViewRoleAttribute : BLLViewRoleBaseAttribute
{
    private static readonly IReadOnlyCollection<MainDTOType> Values = EnumHelper.GetValues<MainDTOType>().ToArray();


    public BLLViewRoleAttribute()
    {
        this.MaxSingle = MainDTOType.RichDTO;
        this.MaxCollection = MainDTOType.FullDTO;
    }


    public MainDTOType MaxFetch { get; set; } = MainDTOType.FullDTO;

    public MainDTOType MaxSingle
    {
        get { return this.Single.OrderByDescending(v => v).First(); }
        set { this.Single = Values.TakeWhile(v => v <= value).ToArray(); }
    }

    public MainDTOType MaxCollection
    {
        get { return this.Collection.OrderByDescending(v => v).First(); }
        set { this.Collection = Values.TakeWhile(v => v <= value).ToArray(); }
    }

    public IReadOnlyList<MainDTOType> Single { get; set; }

    public IReadOnlyList<MainDTOType> Collection { get; set; }

    public IEnumerable<MainDTOType> All => this.Single.Concat(this.Collection);

    public MainDTOType Max
    {
        get { return this.All.Concat(new MainDTOType[] { 0 }).Max(); }
        set
        {
            this.MaxSingle = value;
            this.MaxCollection = value;
        }
    }


    protected override ViewDTOType BaseMaxFetch => (ViewDTOType)this.MaxFetch;
}
