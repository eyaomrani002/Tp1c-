using System.Collections.Generic;
using System.Linq;
using WebApplicationTp1.Models;
using WebApplicationTp1.Models.Repositories;

namespace WebApplicationTp1.Models.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private List<Employee> employees;

        public EmployeeRepository()
        {
            employees = new List<Employee>
            {
                new Employee {Id=1, Name="Sofien ben ali", Department="comptabilité", Salary=1000},
                new Employee {Id=2, Name="Mourad triki", Department="RH", Salary=1500},
                new Employee {Id=3, Name="ali ben mohamed", Department="informatique", Salary=1700},
                new Employee {Id=4, Name="tarak aribi", Department="informatique", Salary=1100},
                new Employee {Id=5, Name="aya omrani", Department="marketing", Salary=1200}

            };
        }

        public void Add(Employee e)
        {
            employees.Add(e);
        }

        public Employee FindByID(int id)
        {
            return employees.FirstOrDefault(a => a.Id == id);
        }

        public void Delete(int id)
        {
            var emp = FindByID(id);
            if (emp != null)
                employees.Remove(emp);
        }

        public IList<Employee> GetAll()
        {
            return employees;
        }

        public void Update(int id, Employee newEmployee)
        {
            var emp = FindByID(id);
            if (emp != null)
            {
                emp.Name = newEmployee.Name;
                emp.Department = newEmployee.Department;
                emp.Salary = newEmployee.Salary;
            }
        }

        public List<Employee> Search(string term)
        {
            if (!string.IsNullOrEmpty(term))
                return employees.Where(a => a.Name.Contains(term)).ToList();
            else
                return employees.ToList();
        }

        public double SalaryAverage()
        {
            return employees.Average(x => x.Salary);
        }

        public double MaxSalary()
        {
            return employees.Max(x => x.Salary);
        }

        public int HrEmployeesCount()
        {
            return employees.Where(x => x.Department == "RH").Count();
        }


        //les methodes pour la filtrage
        public List<Employee> GetByDepartment(string department)
        {
            if (string.IsNullOrEmpty(department) || department == "Tous")
                return employees.ToList();

            return employees.Where(e => e.Department.ToLower() == department.ToLower()).ToList();
        }

        public List<Employee> GetBySalaryRange(int minSalary, int maxSalary)
        {
            return employees.Where(e => e.Salary >= minSalary && e.Salary <= maxSalary).ToList();
        }

        public List<string> GetAllDepartments()
        {
            return employees.Select(e => e.Department)
                           .Distinct()
                           .OrderBy(d => d)
                           .ToList();
        }
    }
}