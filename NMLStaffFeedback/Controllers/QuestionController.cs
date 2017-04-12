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
    public class QuestionController : Controller
    {
        // GET: Question
        NMLStaffFeedbackEntities db = new NMLStaffFeedbackEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var questionSqlRecords = db.Questions.ToList().OrderBy(emp => emp.DateTimeCreated);
            var questionViewRecords = questionSqlRecords.Select(qsr => new QuestionModel(qsr));

            return Json(questionViewRecords.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, QuestionModel questionModel)
        {
            try
            {
               var question = new Question();

               questionModel.DateTimeCreated = DateTime.Now;
               question.InjectFrom(questionModel);                   
               db.Questions.Add(question);
               db.SaveChanges();

               questionModel.ID = question.ID;
            }
            catch (DbEntityValidationException dbEx)
            {
              ModelState.AddDbEntityValidationErrors(dbEx);
            }

            return Json(new[] { questionModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, QuestionModel questionModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = new Question();
                    employee.InjectFrom(questionModel);
                    db.Entry(employee).State = EntityState.Modified;
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
            return Json(new[] { questionModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, QuestionModel questionModel)
        {
            try
            {
                var question = db.Questions.Find(questionModel.ID);

                if (question != null)
                {
                    // Check if employee has already replied to any questions
                    var isAnswerUsed = question.Answers.Any();

                    if (isAnswerUsed)
                    {
                        foreach (var response in question.Answers.ToList())
                        {
                            db.Answers.Remove(response);
                        }
                    }
                    db.Questions.Remove(question);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0} has already been answered by employees",question.Query));
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddDbEntityValidationErrors(dbEx);
            }

            return Json(new[] { questionModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

    }
}