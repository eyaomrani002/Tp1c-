using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTp1.Models;
using WebApplicationTp1.Models.Repositories;

namespace WebApplicationTp1.Controllers

{
    public class EmployeeController : Controller
    {
        readonly IRepository<Employee> employeeRepository;

        // Injection de dépendance
        public EmployeeController(IRepository<Employee> empRepository)
        {
            employeeRepository = empRepository;
        }

        // GET: EmployeeController
        public ActionResult Index(string department = "")
        {
            IEnumerable<Employee> employees;

            if (!string.IsNullOrEmpty(department) && department != "Tous")
            {
                employees = employeeRepository.GetByDepartment(department);
            }
            else
            {
                employees = employeeRepository.GetAll();
            }

            // Statistiques
            ViewData["EmployeesCount"] = employeeRepository.GetAll().Count;
            ViewData["SalaryAverage"] = employeeRepository.SalaryAverage();
            ViewData["MaxSalary"] = employeeRepository.MaxSalary();
            ViewData["HREmployeesCount"] = employeeRepository.HrEmployeesCount();

            // Données pour les filtres
            ViewData["AllDepartments"] = employeeRepository.GetAllDepartments();
            ViewData["SelectedDepartment"] = department;

            return View(employees);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            var employee = employeeRepository.FindByID(id);
            return View(employee);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = employeeRepository.FindByID(id);
            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee employee)
        {
            try
            {
                employeeRepository.Update(id, employee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            var employee = employeeRepository.FindByID(id);
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                employeeRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // Nouvelle action pour le filtre
        [HttpPost]
        public ActionResult Filter(string department, int? minSalary, int? maxSalary)
        {
            IEnumerable<Employee> filteredEmployees;

            if (!string.IsNullOrEmpty(department) && department != "Tous")
            {
                filteredEmployees = employeeRepository.GetByDepartment(department);
            }
            else if (minSalary.HasValue && maxSalary.HasValue)
            {
                filteredEmployees = employeeRepository.GetBySalaryRange(minSalary.Value, maxSalary.Value);
            }
            else
            {
                filteredEmployees = employeeRepository.GetAll();
            }

            ViewData["AllDepartments"] = employeeRepository.GetAllDepartments();
            ViewData["SelectedDepartment"] = department;
            ViewData["EmployeesCount"] = employeeRepository.GetAll().Count;
            ViewData["SalaryAverage"] = employeeRepository.SalaryAverage();
            ViewData["MaxSalary"] = employeeRepository.MaxSalary();
            ViewData["HREmployeesCount"] = employeeRepository.HrEmployeesCount();

            return View("Index", filteredEmployees);
        }
        public ActionResult Search(string term)
        {
            var result = employeeRepository.Search(term);
            ViewData["EmployeesCount"] = result.Count;
            ViewData["SalaryAverage"] = employeeRepository.SalaryAverage();
            ViewData["MaxSalary"] = employeeRepository.MaxSalary();
            ViewData["HREmployeesCount"] = employeeRepository.HrEmployeesCount();
            return View("Index", result);
        }
    }
}