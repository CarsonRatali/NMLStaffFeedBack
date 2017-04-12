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
using System.Web.Security;

namespace NMLStaffFeedback.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        NMLStaffFeedbackEntities db = new NMLStaffFeedbackEntities();
        // GET: Feedback
        public ActionResult Index()
        {
            ViewData["Employee"] = db.Employees.Select(e => new
            {
                ID = e.ID,
                Name = e.FirstName + " " + e.Surname 
            }).OrderBy(o => o.Name);

            ViewData["Questions"] = db.Questions.Select(q => new
            {
                ID = q.ID,
                Question = q.Query
            }).OrderBy(o => o.Question);

            return View();
        }
       
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int employeeID = Int32.Parse(ticket.CookiePath);

            var answerSqlRecords = db.Answers.Where(a => a.Employee_ID == employeeID).ToList();
            //Check for users who have not answered and build list
            var questionsList = db.Questions.ToList();

            foreach (var question in questionsList)
            {
                var questionsAnswered = answerSqlRecords.Any(avr => avr.Question_ID == question.ID);

                if (!questionsAnswered)
                {
                    var outstandingQuestions = new Answer()
                    {
                        AnswerID = 0,
                        Question = question,
                        Employee_ID = employeeID,
                        Question_ID = question.ID,
                        Response = null,
                    };

                    answerSqlRecords.Add(outstandingQuestions);
                }
            }

            var answerViewRecords = answerSqlRecords.OrderBy(asr => asr.Question.DateTimeCreated).Select(asr => new AnswerModel(asr));
            return Json(answerViewRecords.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Answer> answerModel)
        {
            try
            {
                if (answerModel != null)
                {
                    foreach (var answer in answerModel)
                    {
                        var checkIfAnswered = db.Answers.Any(a => (a.Question_ID == answer.Question_ID) &&
                                                                     (a.Employee_ID == answer.Employee_ID));

                        if (!checkIfAnswered)
                        {
                            var newAnswer = new Answer();
                            newAnswer.InjectFrom(answer);
                            db.Answers.Add(newAnswer);
                            db.SaveChanges();

                            answer.AnswerID = newAnswer.AnswerID;
                        }
                       
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }
            return Json(answerModel.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Answer> answerModel)
        {
            try
            {
                if (answerModel != null)
                {
                    foreach (var answer in answerModel)
                    {
                        var updatedAnswer = new Answer();
                        updatedAnswer.InjectFrom(answer);
                        db.Entry(updatedAnswer).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }

            return Json(answerModel.ToDataSourceResult(request, ModelState));
        }
    }
}