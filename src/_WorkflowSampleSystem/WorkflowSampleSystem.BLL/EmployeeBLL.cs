using System;
using System.Collections.Generic;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Models.Filters;

namespace WorkflowSampleSystem.BLL
{
    public partial class EmployeeBLL
    {
        public Employee ChangeByEmail(EmployeeEmailChangeModel changeModel)
        {
            throw new NotImplementedException();
        }

        public EmployeeEmailChangeModel GetChangeByEmail(Employee employee)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetListBy(EmployeeFilterModel filter, IFetchContainer<Employee> fetchs)
        {
            throw new NotImplementedException();
        }

        public EmployeeEmailMassChangeModel GetMassChangeByEmail(List<Employee> employees)
        {
            if (employees == null) throw new ArgumentNullException(nameof(employees));

            return new EmployeeEmailMassChangeModel { ChangingObjects = employees };
        }

        public Employee IntegrationSave(EmployeeCustomIntegrationSaveModel integrationSaveModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Изменением всем сотрудникам из модели email
        /// </summary>
        /// <param name="changeModel"></param>
        /// <returns></returns>
        public List<Employee> MassChangeByEmail(EmployeeEmailMassChangeModel changeModel)
        {
            if (changeModel == null) throw new ArgumentNullException(nameof(changeModel));

            changeModel.ChangingObjects.ForEach(employee => employee.Email = changeModel.Email);

            this.Save(changeModel.ChangingObjects);

            return changeModel.ChangingObjects;
        }

        public Employee ComplexChange(EmployeeComplexChangeModel changeModel)
        {
            if (changeModel == null) throw new ArgumentNullException(nameof(changeModel));

            changeModel.PrimaryChangingObject.Email = changeModel.Email;

            this.Save(changeModel.PrimaryChangingObject);

            changeModel.SecondaryChangingObjects.ForEach(employee => employee.Email = changeModel.Email);

            this.Save(changeModel.SecondaryChangingObjects);

            return changeModel.PrimaryChangingObject;
        }
    }
}
