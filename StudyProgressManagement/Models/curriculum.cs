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
    
    public partial class curriculum
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public curriculum()
        {
            this.registration_results = new HashSet<registration_results>();
            this.study_results = new HashSet<study_results>();
        }
    
        public int id { get; set; }
        public string curriculum_id { get; set; }
        public string name { get; set; }
        public string name_english { get; set; }
        public int credits { get; set; }
        public string education_level { get; set; }
        public string education_type { get; set; }
        public Nullable<int> theoretical_hours { get; set; }
        public Nullable<int> practice_hours { get; set; }
        public Nullable<int> internship_hours { get; set; }
        public Nullable<int> project_hours { get; set; }
        public string compulsory_or_optional { get; set; }
        public string prerequisites { get; set; }
        public string learn_before { get; set; }
        public string editing_notes { get; set; }
        public string knowledge_type_alias { get; set; }
        public string group_1 { get; set; }
        public string group_2 { get; set; }
        public string group_3 { get; set; }
        public int student_course_id { get; set; }
    
        public virtual student_course student_course { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<registration_results> registration_results { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<study_results> study_results { get; set; }
    }
}
