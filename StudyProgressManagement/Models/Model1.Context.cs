﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

    public partial class sep_team03Entities : DbContext
    {
        public sep_team03Entities()
            : base("name=sep_team03Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<class_student> class_student { get; set; }
        public virtual DbSet<curriculum> curricula { get; set; }
        public virtual DbSet<curriculum_class> curriculum_class { get; set; }
        public virtual DbSet<lecturer> lecturers { get; set; }
        public virtual DbSet<major> majors { get; set; }
        public virtual DbSet<registration_results> registration_results { get; set; }
        public virtual DbSet<student> students { get; set; }
        public virtual DbSet<student_course> student_course { get; set; }
        public virtual DbSet<studentcourse_curriculum> studentcourse_curriculum { get; set; }
        public virtual DbSet<study_results> study_results { get; set; }
        public virtual DbSet<study_unit> study_unit { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<term> terms { get; set; }
    }
}
