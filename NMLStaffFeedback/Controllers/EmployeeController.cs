using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NMLStaffFeedback.Data;
using Kendo.Mvc.UI;
using NMLStaffFeedback.Models;
using Omu.ValueInjecter;
using System.Data.Entity.Validation;
using NMLStaffFeedback.Helpers;
using System.Data.Entity;

namespace NMLStaffFeedback.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        NMLStaffFeedbackEntities db = new NMLStaffFeedbackEntities();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var employeeSqlRecords = db.Employees.ToList().OrderBy(emp => emp.Surname);

            var employeeViewRecords = employeeSqlRecords.Select(esr => new EmployeesModel(esr));

            return Json(employeeViewRecords.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, EmployeesModel employeeModel)
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
            return Json(new[] { employeeModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, EmployeesModel employeeModel)
        {
            try
            {
                // Check if employee exists
                var doesEmployeeExist = db.Employees.Any(emp => (emp.FirstName == employeeModel.FirstName) &&
                                                                (emp.Surname == employeeModel.Surname) &&
                                                                (emp.Email == employeeModel.Email) &&
                                                                (emp.ID != employeeModel.ID));

                if (!doesEmployeeExist && ModelState.IsValid)
                {
                    var employee = new Employee();
                    employee.InjectFrom(employeeModel);
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
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
            return Json(new[] { employeeModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, EmployeesModel employeeModel)
        {
            try
            {
                var employee = db.Employees.Find(employeeModel.ID);

                if (employee != null)
                {
                    // Check if employee has already replied to any questions
                    var isEmployeeUsed = employee.Answers.Any();

                    if (!isEmployeeUsed)
                    {
                        db.Employees.Remove(employee);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("Employee {0} {1} with email address {2} is in use.", employeeModel.FirstName, employeeModel.Surname, employeeModel.Email));
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }

            return Json(new[] { employeeModel }.ToDataSourceResult(request, ModelState));
        }

    }
}