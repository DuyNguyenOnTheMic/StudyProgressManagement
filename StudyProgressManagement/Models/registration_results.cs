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
    
    public partial class registration_results
    {
        public int id { get; set; }
        public string curriculum_id { get; set; }
        public string curriculum_class_id { get; set; }
        public string curriculum_name { get; set; }
        public int credits { get; set; }
        public string registration_type { get; set; }
        public string education_level { get; set; }
        public string education_type { get; set; }
        public System.DateTime registration_date { get; set; }
        public string registration_person { get; set; }
        public string lecturer_id { get; set; }
        public string lecturer_name { get; set; }
        public string schedule { get; set; }
        public string student_id { get; set; }
    
        public virtual student student { get; set; }
    }
}
