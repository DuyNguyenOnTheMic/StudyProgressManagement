//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudyProgressManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class study_results
    {
        public int id { get; set; }
        public string term_id { get; set; }
        public string term_name { get; set; }
        public string curriculum_id { get; set; }
        public string study_unit_id { get; set; }
        public string study_unit_alias { get; set; }
        public string curriculum_name { get; set; }
        public int credits { get; set; }
        public string mark10 { get; set; }
        public string mark10_2 { get; set; }
        public string mark10_3 { get; set; }
        public string mark10_4 { get; set; }
        public string mark10_5 { get; set; }
        public string max_mark_10 { get; set; }
        public string max_mark_4 { get; set; }
        public string max_mark_letter { get; set; }
        public string is_pass { get; set; }
        public Nullable<int> major_id { get; set; }
        public Nullable<int> student_course_id { get; set; }
        public string student_id { get; set; }
    
        public virtual major major { get; set; }
        public virtual student student { get; set; }
        public virtual student_course student_course { get; set; }
    }
}
