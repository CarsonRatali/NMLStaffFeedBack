using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NMLStaffFeedback.Data;
using Kendo.Mvc.UI;
using NMLStaffFeedback.Models;

namespace NMLStaffFeedback.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        NMLStaffFeedbackEntities db = new NMLStaffFeedbackEntities();
        // GET: Answers
        public ActionResult Index()
        {
            ViewData["Employee"] = db.Employees.Select(e => new
            {
                ID = e.ID,
                Name =  e.FirstName +" "+ e.Surname
            }).OrderBy(o => o.Name);

            ViewData["Questions"] = db.Questions.Select(q => new
            {
                ID = q.ID,
                Question = q.Query
            }).OrderBy(o => o.Question);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request, int? questionID)
        {
            var answerSqlRecords = db.Answers.Where(a => a.Question_ID == questionID).ToList();

            //Check for emoloyees who have not answered and build list
            var employeeList = db.Employees.ToList();

            foreach (var employee in employeeList)
            {
                var employeeHasAnswered = answerSqlRecords.Any(avr => avr.Employee_ID == employee.ID);

                if (!employeeHasAnswered)
                {
                    var newEmployeeAnswer = new Answer()
                    {
                        AnswerID = 0,                      
                        Employee = employee,
                        Question_ID = questionID ?? 0,
                        Employee_ID = employee.ID,
                        Response = null,                    
                    };

                    answerSqlRecords.Add(newEmployeeAnswer);
                }
            }

            var answerViewRecords = answerSqlRecords.OrderBy(emp => emp.Employee.FirstName).Select(asr => new AnswerModel(asr));
            return Json(answerViewRecords.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}