using Kendo.Mvc.UI;
using NMLStaffFeedback.Data;
using NMLStaffFeedback.Helpers;
using NMLStaffFeedback.Models;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NMLStaffFeedback.Controllers
{
    public class LoginController : Controller
    {
        NMLStaffFeedbackEntities db = new NMLStaffFeedbackEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNewEmployee([DataSourceRequest] DataSourceRequest request, EmployeesModel employeeModel)
        {
            try
            {
                // Check if employee exists
                var doesEmployeeExist = db.Employees.Any(emp => (emp.FirstName == employeeModel.FirstName) &&
                                                                (emp.Surname == employeeModel.Surname) &&
                                                                (emp.Email == employeeModel.Email));

                if (!doesEmployeeExist)
                {
                    var employee = new Employee();
                    employee.InjectFrom(employeeModel);

                    db.Employees.Add(employee);
                    db.SaveChanges();

                    employeeModel.ID = employee.ID;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("Employee {0} {1} with email address {2} already exists", employeeModel.FirstName, employeeModel.Surname, employeeModel.Email));
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }
            return RedirectToAction("Login", "Login");         
        }

        [HttpPost]
        public ActionResult Login(EmployeesModel employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.IsValid(employee.FirstName, employee.Email))
                {
                    var employeeID = db.Employees.Where(emp => emp.FirstName == employee.FirstName && emp.Email == employee.Email).Select(a=>a.ID).First();
                    FormsAuthentication.SetAuthCookie(employee.FirstName, false, employeeID.ToString());
                    return RedirectToAction("Index", "Feedback");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(employee);
        }

        public ActionResult Logout()
        {          
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}
