//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMLStaffFeedback.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answer
    {
        public int AnswerID { get; set; }
        public int Employee_ID { get; set; }
        public int Question_ID { get; set; }
        public Nullable<bool> Response { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Question Question { get; set; }
    }
}
