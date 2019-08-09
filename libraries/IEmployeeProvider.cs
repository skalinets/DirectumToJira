using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    interface IEmployeeProvider
    {
        List<Employee> GetEmployees();
    }
    class EmployeeProvider : IEmployeeProvider
    {
        private string employeeEndpointName;

        public EmployeeProvider(string employeeEndpointName)
        {
            this.employeeEndpointName = employeeEndpointName;
        }

        public List<Employee> GetEmployees()
        {
            throw new NotImplementedException();
        }
    }
}
