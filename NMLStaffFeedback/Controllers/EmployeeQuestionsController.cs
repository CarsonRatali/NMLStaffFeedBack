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
    public class EmployeeQuestionsController : Controller
    {
        NMLStaffFeedbackEntities db = new NMLStaffFeedbackEntities();
        // GET: EmployeeQuestions
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request, int? employeeID)
        {
            var answerSqlRecords = db.Answers.Where(a => a.Employee_ID == employeeID).ToList();

            //Check for users who have not answered and build list
            var questionList = db.Questions.ToList();

            foreach (var question in questionList)
            {
                var isQuestionAnswered = answerSqlRecords.Any(asr => asr.Question_ID == question.ID);

                if (!isQuestionAnswered)
                {
                    var outstandingQuestion = new Answer()
                    {
                        Question = question,
                        Question_ID = question.ID ,
                        Employee_ID = employeeID ?? 0,
                        Response = null,
                    };
                    answerSqlRecords.Add(outstandingQuestion);
                }
            }
            var answerViewRecords = answerSqlRecords.OrderBy(asr => asr.Question.DateTimeCreated).Select(asr => new AnswerModel(asr));
            return Json(answerViewRecords.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, AnswerModel answerModel)
        {
            try
            {
                var answer = new Answer();

                answer.InjectFrom(answerModel);
                db.Answers.Add(answer);
                db.SaveChanges();
            }            
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }

            return Json(new[] { answerModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, AnswerModel answerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var answer = new Answer();
                    answer.InjectFrom(answerModel);
                    db.Entry(answer).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("One or more fields are empty"));
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }
            return Json(new[] { answerModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
    }
}