using FluentNHibernate.Mapping;

using SampleSystem.Domain;

namespace SampleSystem.Generated.DAL.NHibernate.FluentMapping;

public class ManagementUnitFluentMappingMap : ClassMap<ManagementUnitFluentMapping>
{
    public ManagementUnitFluentMappingMap()
    {
        this.Schema("app");
        this.Id(x => x.Id).GeneratedBy.GuidComb();
        this.Version(x => x.Version).Generated.Never();
        this.Map(x => x.Active).Access.CamelCaseField();
        this.Map(x => x.BusinessUnitStatus).Access.CamelCaseField();
        this.Map(x => x.CreateDate).Access.CamelCaseField();
        this.Map(x => x.CreatedBy).Access.CamelCaseField();
        this.Map(x => x.IsProduction).Access.CamelCaseField();
        this.Map(x => x.ModifiedBy).Access.CamelCaseField();
        this.Map(x => x.ModifyDate).Access.CamelCaseField();
        this.Map(x => x.Name).Access.CamelCaseField();
        this.References(x => x.Parent).Access.CamelCaseField().Column("parentId");
        this.HasMany(x => x.Children).Inverse().Cascade.None().Access.CamelCaseField();
        this.Component(
                       x => x.Period,
                       m =>
                       {
                           m.Map(x => x.EndDate).Column("periodendDate111").Access.CamelCaseField().CustomType("timestamp");
                           m.Map(x => x.StartDate).Column("periodstartDate111").Access.CamelCaseField().CustomType("timestamp");
                       })
            .Access.CamelCaseField();
        this.Component(
                       x => x.MuComponent,
                       m =>
                       {
                           m.Map(x => x.LuxoftSignsFirst, "muComponentluxoftSignsFirst222");
                           m.References(x => x.AuthorizedLuxoftSignatory).Columns("muComponentauthorizedLuxoftSignatoryId");
                       });
    }
    /*
<class name="SampleSystem.Domain.ManagementUnitFluentMapping" table="[ManagementUnitFluentMapping]">
<id name="Id" column="[id]" type="Guid" access="field.camelcase">
  <generator class="guid.comb" />
</id>
<version name="Version" generated="never" type="Int64">
  <column name="Version" not-null="false" sql-type="bigint" />
</version>
<property name="Active" column="[active]" access="field.camelcase" />
<property name="BusinessUnitStatus" column="[businessUnitStatus]" access="field.camelcase" />
<property name="CreateDate" column="[createDate]" type="timestamp" access="field.camelcase" />
<property name="CreatedBy" column="[createdBy]" access="field.camelcase" />
<property name="IsProduction" column="[isProduction]" access="field.camelcase" />
<property name="ModifiedBy" column="[modifiedBy]" access="field.camelcase" />
<property name="ModifyDate" column="[modifyDate]" type="timestamp" access="field.camelcase" />
<property name="Name" column="[name]" access="field.camelcase" />
<many-to-one name="Parent" column="[parentId]" class="SampleSystem.Domain.ManagementUnitFluentMapping" access="field.camelcase" />
<set name="Children" inverse="true" access="field.camelcase" cascade="None">
  <key column="[parentId]" />
  <one-to-many class="SampleSystem.Domain.ManagementUnitFluentMapping" />
</set>
<component name="Period" class="Framework.Core.Period, Framework.Core" access="field.camelcase">
  <property name="EndDate" column="[periodendDate]" access="field.camelcase" type="timestamp" />
  <property name="StartDate" column="[periodstartDate]" access="field.camelcase" type="timestamp" />
</component>
</class>
*/
}
