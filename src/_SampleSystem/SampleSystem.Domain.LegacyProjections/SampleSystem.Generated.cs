﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleSystem.Domain.Projections
{
    
    
    [Framework.Persistent.Mapping.TableAttribute(Name="BusinessUnit")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.BusinessUnit), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityBusinessUnit : SampleSystem.Domain.PersistentDomainObjectBase, Framework.SecuritySystem.ISecurityContext, SampleSystem.Domain.IBusinessUnitSecurityElement<SampleSystem.Domain.Projections.SecurityBusinessUnit>, Framework.Persistent.IDenormalizedHierarchicalPersistentSource<SampleSystem.Domain.Projections.SecurityBusinessUnitAncestorLink, SampleSystem.Domain.Projections.SecurityBusinessUnitToAncestorChildView, SampleSystem.Domain.Projections.SecurityBusinessUnit, System.Guid>, Framework.Persistent.IHierarchicalPersistentDomainObjectBase<SampleSystem.Domain.Projections.SecurityBusinessUnit, System.Guid>, Framework.Persistent.IHierarchicalSource<SampleSystem.Domain.Projections.SecurityBusinessUnit>, Framework.Persistent.IParentSource<SampleSystem.Domain.Projections.SecurityBusinessUnit>, Framework.Persistent.IChildrenSource<SampleSystem.Domain.Projections.SecurityBusinessUnit>
    {
        
        private System.Collections.Generic.ICollection<SampleSystem.Domain.Projections.SecurityBusinessUnit> children_Security;
        
        private SampleSystem.Domain.Projections.SecurityBusinessUnit parent_Security;
        
        protected SecurityBusinessUnit()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit SampleSystem.Domain.IBusinessUnitSecurityElement<SampleSystem.Domain.Projections.SecurityBusinessUnit>.BusinessUnit
        {
            get
            {
                return this;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Children_Security")]
        System.Collections.Generic.IEnumerable<SampleSystem.Domain.Projections.SecurityBusinessUnit> Framework.Persistent.IChildrenSource<SampleSystem.Domain.Projections.SecurityBusinessUnit>.Children
        {
            get
            {
                return this.Children_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Domain.Projections.SecurityBusinessUnit> Children_Security
        {
            get
            {
                return this.children_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Parent_Security")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit Framework.Persistent.IParentSource<SampleSystem.Domain.Projections.SecurityBusinessUnit>.Parent
        {
            get
            {
                return this.Parent_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="parentId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityBusinessUnit Parent_Security
        {
            get
            {
                return this.parent_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="BusinessUnitAncestorLink")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.BusinessUnitAncestorLink), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityBusinessUnitAncestorLink : SampleSystem.Domain.PersistentDomainObjectBase, Framework.Persistent.IHierarchicalAncestorLink<SampleSystem.Domain.Projections.SecurityBusinessUnit, SampleSystem.Domain.Projections.SecurityBusinessUnitToAncestorChildView, System.Guid>
    {
        
        private SampleSystem.Domain.Projections.SecurityBusinessUnit ancestor_Security;
        
        private SampleSystem.Domain.Projections.SecurityBusinessUnit child_Security;
        
        protected SecurityBusinessUnitAncestorLink()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("Ancestor_Security")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit Framework.Persistent.IHierarchicalAncestorLink<SampleSystem.Domain.Projections.SecurityBusinessUnit, SampleSystem.Domain.Projections.SecurityBusinessUnitToAncestorChildView, System.Guid>.Ancestor
        {
            get
            {
                return this.Ancestor_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="ancestorId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityBusinessUnit Ancestor_Security
        {
            get
            {
                return this.ancestor_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Child_Security")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit Framework.Persistent.IHierarchicalAncestorLink<SampleSystem.Domain.Projections.SecurityBusinessUnit, SampleSystem.Domain.Projections.SecurityBusinessUnitToAncestorChildView, System.Guid>.Child
        {
            get
            {
                return this.Child_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="childId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityBusinessUnit Child_Security
        {
            get
            {
                return this.child_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="BusinessUnitToAncestorChildView")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.BusinessUnitToAncestorChildView), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityBusinessUnitToAncestorChildView : SampleSystem.Domain.PersistentDomainObjectBase, Framework.Persistent.IHierarchicalToAncestorOrChildLink<SampleSystem.Domain.Projections.SecurityBusinessUnit, System.Guid>
    {
        
        private SampleSystem.Domain.Projections.SecurityBusinessUnit childOrAncestor_Security;
        
        private SampleSystem.Domain.Projections.SecurityBusinessUnit source_Security;
        
        protected SecurityBusinessUnitToAncestorChildView()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("ChildOrAncestor_Security")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit Framework.Persistent.IHierarchicalToAncestorOrChildLink<SampleSystem.Domain.Projections.SecurityBusinessUnit, System.Guid>.ChildOrAncestor
        {
            get
            {
                return this.ChildOrAncestor_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="childOrAncestorId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityBusinessUnit ChildOrAncestor_Security
        {
            get
            {
                return this.childOrAncestor_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Source_Security")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit Framework.Persistent.IHierarchicalToAncestorOrChildLink<SampleSystem.Domain.Projections.SecurityBusinessUnit, System.Guid>.Source
        {
            get
            {
                return this.Source_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="sourceId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityBusinessUnit Source_Security
        {
            get
            {
                return this.source_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="Employee")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.Employee), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityEmployee : SampleSystem.Domain.PersistentDomainObjectBase, Framework.SecuritySystem.ISecurityContext, SampleSystem.Domain.IEmployeeSecurity<SampleSystem.Domain.Projections.SecurityBusinessUnit, SampleSystem.Domain.Projections.SecurityHRDepartment, SampleSystem.Domain.Projections.SecurityLocation>, SampleSystem.Domain.IBusinessUnitSecurityElement<SampleSystem.Domain.Projections.SecurityBusinessUnit>, SampleSystem.Domain.IDepartmentSecurityElement<SampleSystem.Domain.Projections.SecurityHRDepartment>, SampleSystem.Domain.IEmployeeSecurityElement<SampleSystem.Domain.Projections.SecurityEmployee, SampleSystem.Domain.Projections.SecurityBusinessUnit, SampleSystem.Domain.Projections.SecurityHRDepartment, SampleSystem.Domain.Projections.SecurityLocation>, SampleSystem.Domain.IEmployeeSecurityElement<SampleSystem.Domain.Projections.SecurityEmployee>
    {
        
        private SampleSystem.Domain.Projections.SecurityBusinessUnit businessUnit_Security;
        
        private SampleSystem.Domain.Projections.SecurityHRDepartment department_Security;
        
        private string login_Security;
        
        protected SecurityEmployee()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("BusinessUnit_Security")]
        SampleSystem.Domain.Projections.SecurityBusinessUnit SampleSystem.Domain.IBusinessUnitSecurityElement<SampleSystem.Domain.Projections.SecurityBusinessUnit>.BusinessUnit
        {
            get
            {
                return this.BusinessUnit_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="coreBusinessUnitId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityBusinessUnit BusinessUnit_Security
        {
            get
            {
                return this.businessUnit_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Department_Security")]
        SampleSystem.Domain.Projections.SecurityHRDepartment SampleSystem.Domain.IDepartmentSecurityElement<SampleSystem.Domain.Projections.SecurityHRDepartment>.Department
        {
            get
            {
                return this.Department_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="hRDepartmentId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityHRDepartment Department_Security
        {
            get
            {
                return this.department_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("")]
        SampleSystem.Domain.Projections.SecurityEmployee SampleSystem.Domain.IEmployeeSecurityElement<SampleSystem.Domain.Projections.SecurityEmployee>.Employee
        {
            get
            {
                return this;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Login_Security")]
        string SampleSystem.Domain.IEmployeeSecurity<SampleSystem.Domain.Projections.SecurityBusinessUnit, SampleSystem.Domain.Projections.SecurityHRDepartment, SampleSystem.Domain.Projections.SecurityLocation>.Login
        {
            get
            {
                return this.Login_Security;
            }
        }
        
        [Framework.Security.ViewDomainObjectAttribute(typeof(SampleSystem.SampleSystemSecurityOperation), "View")]
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="login")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual string Login_Security
        {
            get
            {
                return this.login_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="HRDepartment")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.HRDepartment), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityHRDepartment : SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Domain.ILocationSecurityElement<SampleSystem.Domain.Projections.SecurityLocation>, Framework.Persistent.IHierarchicalSource<SampleSystem.Domain.Projections.SecurityHRDepartment>, Framework.Persistent.IParentSource<SampleSystem.Domain.Projections.SecurityHRDepartment>, Framework.Persistent.IChildrenSource<SampleSystem.Domain.Projections.SecurityHRDepartment>
    {
        
        private System.Collections.Generic.ICollection<SampleSystem.Domain.Projections.SecurityHRDepartment> children_Security;
        
        private SampleSystem.Domain.Projections.SecurityLocation location_Security;
        
        private SampleSystem.Domain.Projections.SecurityHRDepartment parent_Security;
        
        protected SecurityHRDepartment()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("Children_Security")]
        System.Collections.Generic.IEnumerable<SampleSystem.Domain.Projections.SecurityHRDepartment> Framework.Persistent.IChildrenSource<SampleSystem.Domain.Projections.SecurityHRDepartment>.Children
        {
            get
            {
                return this.Children_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Domain.Projections.SecurityHRDepartment> Children_Security
        {
            get
            {
                return this.children_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Location_Security")]
        SampleSystem.Domain.Projections.SecurityLocation SampleSystem.Domain.ILocationSecurityElement<SampleSystem.Domain.Projections.SecurityLocation>.Location
        {
            get
            {
                return this.Location_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="locationId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityLocation Location_Security
        {
            get
            {
                return this.location_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Parent_Security")]
        SampleSystem.Domain.Projections.SecurityHRDepartment Framework.Persistent.IParentSource<SampleSystem.Domain.Projections.SecurityHRDepartment>.Parent
        {
            get
            {
                return this.Parent_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="parentId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityHRDepartment Parent_Security
        {
            get
            {
                return this.parent_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="Location")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.Location), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityLocation : SampleSystem.Domain.PersistentDomainObjectBase, Framework.SecuritySystem.ISecurityContext, Framework.Persistent.IDenormalizedHierarchicalPersistentSource<SampleSystem.Domain.Projections.SecurityLocationAncestorLink, SampleSystem.Domain.Projections.SecurityLocationToAncestorChildView, SampleSystem.Domain.Projections.SecurityLocation, System.Guid>, Framework.Persistent.IHierarchicalPersistentDomainObjectBase<SampleSystem.Domain.Projections.SecurityLocation, System.Guid>, Framework.Persistent.IHierarchicalSource<SampleSystem.Domain.Projections.SecurityLocation>, Framework.Persistent.IParentSource<SampleSystem.Domain.Projections.SecurityLocation>, Framework.Persistent.IChildrenSource<SampleSystem.Domain.Projections.SecurityLocation>
    {
        
        private System.Collections.Generic.ICollection<SampleSystem.Domain.Projections.SecurityLocation> children_Security;
        
        private SampleSystem.Domain.Projections.SecurityLocation parent_Security;
        
        protected SecurityLocation()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("Children_Security")]
        System.Collections.Generic.IEnumerable<SampleSystem.Domain.Projections.SecurityLocation> Framework.Persistent.IChildrenSource<SampleSystem.Domain.Projections.SecurityLocation>.Children
        {
            get
            {
                return this.Children_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Domain.Projections.SecurityLocation> Children_Security
        {
            get
            {
                return this.children_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Parent_Security")]
        SampleSystem.Domain.Projections.SecurityLocation Framework.Persistent.IParentSource<SampleSystem.Domain.Projections.SecurityLocation>.Parent
        {
            get
            {
                return this.Parent_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="parentId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityLocation Parent_Security
        {
            get
            {
                return this.parent_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="LocationAncestorLink")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.LocationAncestorLink), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityLocationAncestorLink : SampleSystem.Domain.PersistentDomainObjectBase, Framework.Persistent.IHierarchicalAncestorLink<SampleSystem.Domain.Projections.SecurityLocation, SampleSystem.Domain.Projections.SecurityLocationToAncestorChildView, System.Guid>
    {
        
        private SampleSystem.Domain.Projections.SecurityLocation ancestor_Security;
        
        private SampleSystem.Domain.Projections.SecurityLocation child_Security;
        
        protected SecurityLocationAncestorLink()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("Ancestor_Security")]
        SampleSystem.Domain.Projections.SecurityLocation Framework.Persistent.IHierarchicalAncestorLink<SampleSystem.Domain.Projections.SecurityLocation, SampleSystem.Domain.Projections.SecurityLocationToAncestorChildView, System.Guid>.Ancestor
        {
            get
            {
                return this.Ancestor_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="ancestorId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityLocation Ancestor_Security
        {
            get
            {
                return this.ancestor_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Child_Security")]
        SampleSystem.Domain.Projections.SecurityLocation Framework.Persistent.IHierarchicalAncestorLink<SampleSystem.Domain.Projections.SecurityLocation, SampleSystem.Domain.Projections.SecurityLocationToAncestorChildView, System.Guid>.Child
        {
            get
            {
                return this.Child_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="childId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityLocation Child_Security
        {
            get
            {
                return this.child_Security;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="LocationToAncestorChildView")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.LocationToAncestorChildView), Framework.Projection.ProjectionRole.SecurityNode)]
    public partial class SecurityLocationToAncestorChildView : SampleSystem.Domain.PersistentDomainObjectBase, Framework.Persistent.IHierarchicalToAncestorOrChildLink<SampleSystem.Domain.Projections.SecurityLocation, System.Guid>
    {
        
        private SampleSystem.Domain.Projections.SecurityLocation childOrAncestor_Security;
        
        private SampleSystem.Domain.Projections.SecurityLocation source_Security;
        
        protected SecurityLocationToAncestorChildView()
        {
        }
        
        [Framework.Persistent.ExpandPathAttribute("ChildOrAncestor_Security")]
        SampleSystem.Domain.Projections.SecurityLocation Framework.Persistent.IHierarchicalToAncestorOrChildLink<SampleSystem.Domain.Projections.SecurityLocation, System.Guid>.ChildOrAncestor
        {
            get
            {
                return this.ChildOrAncestor_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="childOrAncestorId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityLocation ChildOrAncestor_Security
        {
            get
            {
                return this.childOrAncestor_Security;
            }
        }
        
        [Framework.Persistent.ExpandPathAttribute("Source_Security")]
        SampleSystem.Domain.Projections.SecurityLocation Framework.Persistent.IHierarchicalToAncestorOrChildLink<SampleSystem.Domain.Projections.SecurityLocation, System.Guid>.Source
        {
            get
            {
                return this.Source_Security;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Security)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="sourceId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.SecurityLocation Source_Security
        {
            get
            {
                return this.source_Security;
            }
        }
    }
    
    [Framework.DomainDriven.BLL.BLLProjectionViewRoleAttribute()]
    [Framework.Persistent.Mapping.InlineBaseTypeMappingAttribute()]
    [Framework.Persistent.Mapping.TableAttribute(Name="Employee")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.Employee), Framework.Projection.ProjectionRole.Default)]
    [Framework.Projection.ProjectionFilterAttribute(typeof(SampleSystem.Domain.Models.Filters.EmployeeFilterModel), Framework.Projection.ProjectionFilterTargets.Collection)]
    public partial class TestLegacyEmployee : SampleSystem.Domain.Projections.SecurityEmployee
    {
        
        private string login;
        
        private SampleSystem.Domain.Projections.TestLegacyEmployee_AutoProp_Role role_Auto;
        
        protected TestLegacyEmployee()
        {
        }
        
        [Framework.Security.ViewDomainObjectAttribute(typeof(SampleSystem.SampleSystemSecurityOperation), "View")]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Default)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="login")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual string Login
        {
            get
            {
                return this.login;
            }
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.AutoNode)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="roleId")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual SampleSystem.Domain.Projections.TestLegacyEmployee_AutoProp_Role Role_Auto
        {
            get
            {
                return this.role_Auto;
            }
        }
        
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Default)]
        [Framework.Persistent.ExpandPathAttribute("Role_Auto.Id")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual System.Guid? RoleId
        {
            get
            {
                return this.Role_Auto?.Id;
            }
        }
        
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.Default)]
        [Framework.Persistent.ExpandPathAttribute("Role_Auto.Name_Last_RoleName")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual string RoleName
        {
            get
            {
                return this.Role_Auto?.Name_Last_RoleName;
            }
        }
    }
    
    [Framework.Persistent.Mapping.TableAttribute(Name="EmployeeRole")]
    [Framework.Projection.ProjectionAttribute(typeof(SampleSystem.Domain.EmployeeRole), Framework.Projection.ProjectionRole.AutoNode)]
    public partial class TestLegacyEmployee_AutoProp_Role : SampleSystem.Domain.PersistentDomainObjectBase
    {
        
        private string name_Last_RoleName;
        
        protected TestLegacyEmployee_AutoProp_Role()
        {
        }
        
        [Framework.DomainDriven.Serialization.CustomSerializationAttribute(Framework.DomainDriven.Serialization.CustomSerializationMode.Ignore)]
        [Framework.Projection.ProjectionPropertyAttribute(Framework.Projection.ProjectionPropertyRole.LastAutoNode)]
        [Framework.Persistent.Mapping.MappingAttribute(ColumnName="name")]
        [Framework.Persistent.Mapping.MappingPropertyAttribute(CanInsert=false, CanUpdate=false)]
        public virtual string Name_Last_RoleName
        {
            get
            {
                return this.name_Last_RoleName;
            }
        }
    }
}
