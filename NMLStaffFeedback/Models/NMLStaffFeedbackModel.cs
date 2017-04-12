using NMLStaffFeedback.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using System.ComponentModel.DataAnnotations;

namespace NMLStaffFeedback.Models
{ 
    public class EmployeesModel
    {
        #region Constructors
        public EmployeesModel()
        {

        }

        public EmployeesModel( Employee sqlRecord )
        {
            this.InjectFrom(sqlRecord);                                   
        }

        #endregion

        [ScaffoldColumn(false)]
        public int ID { get; set; }
       
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        // Used for login validtion
        public bool IsValid(string _firstName, string _email)
        {
            using (var db = new NMLStaffFeedbackEntities())
            {
              return (db.Employees.Any(emp => emp.FirstName == _firstName && emp.Email == _email));
            }
        }
    }

    public class QuestionModel
    {
        #region Constructors
        public QuestionModel()
        {
                
        }

        public QuestionModel(Question sqlRecord)
        {
            this.InjectFrom(sqlRecord);
        }
        #endregion

        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string Query { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Date Created")]
        public DateTime? DateTimeCreated { get; set; }
    }

    public class AnswerModel
    {
        #region Constructors
        public AnswerModel()
        {

        }

        public AnswerModel(Answer sqlRecord)
        {
            this.InjectFrom(sqlRecord);
        }
        #endregion

        [ScaffoldColumn(false)]
        public int AnswerID { get; set; }

        [Display(Name = "Employee")]
        public int Employee_ID { get; set; }

        [Display(Name = "Question")]
        public int Question_ID { get; set; }

        [Display(Name = "Answer")]
        public bool? Response { get; set; }
    } 
}