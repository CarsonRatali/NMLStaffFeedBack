using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NMLStaffFeedback.Helpers
{
    public static class ModelStateExtensions
    {
        public static void AddDbEntityValidationErrors(this ModelStateDictionary modelState, DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    modelState.AddModelError(string.Empty, string.Format("{0} - {1}", validationError.PropertyName, validationError.ErrorMessage));
                }
            }
        }

    }
}