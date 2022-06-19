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
    
    public partial class student_course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public student_course()
        {
            this.curricula = new HashSet<curriculum>();
            this.students = new HashSet<student>();
            this.studentcourse_curriculum = new HashSet<studentcourse_curriculum>();
        }
    
        public int id { get; set; }
        public string course { get; set; }
        public string year_study { get; set; }
        public int major_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<curriculum> curricula { get; set; }
        public virtual major major { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<student> students { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<studentcourse_curriculum> studentcourse_curriculum { get; set; }
    }
}
